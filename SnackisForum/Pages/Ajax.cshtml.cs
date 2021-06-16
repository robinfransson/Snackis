using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnackisDB.Models;
using SnackisDB.Models.Identity;
using SnackisForum.Injects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SnackisForum.Pages
{
    public class AjaxModel : PageModel
    {
        #region Constructor and readonlies
        private readonly UserManager<SnackisUser> _userManager;
        private readonly SignInManager<SnackisUser> _signInManager;
        private readonly SnackisContext _context;
        private readonly ILogger<AjaxModel> _logger;
        private readonly UserProfile _userProfile;



        public AjaxModel(UserManager<SnackisUser> userManager, SignInManager<SnackisUser> signInManager,
                        SnackisContext context, ILogger<AjaxModel> logger, UserProfile userProfile)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _logger = logger;
            _userProfile = userProfile;
        }
        #endregion
        public IActionResult OnGet()
        {
            return RedirectToPage("Index");
        }

        #region Make admin
        public async Task<JsonResult> OnPostMakeAdminAsync()
        {
            int adminUsers = _userManager.GetUsersInRoleAsync("Admin").Result.Count;
            if (adminUsers < 1)
            {

                var user = await _userManager.GetUserAsync(User);
                var result = await _userManager.AddToRoleAsync(user, "Admin");
                if (result.Succeeded)
                {
                    return new JsonResult(new { success = true });
                }
                else
                {

                    return new JsonResult(new { success = false });
                }
            }
            return new JsonResult(new { success = false });
        }



        #endregion


        #region Logout
        public async Task<JsonResult> OnPostLogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return new JsonResult(new { loggedout = true });
        }


        #endregion


        #region Register
        public async Task<JsonResult> OnPostRegisterAsync(string email, string username, string password, bool remember)
        {


            //int age = (int)Math.Floor((DateTime.Now - birthDate).TotalDays / 365.25D);
            if (ModelState.IsValid)
            {
                //    if(age < 13)
                //    {
                //        return new JsonResult(new { error = "underage", success = false });
                //    }
                var user = new SnackisUser
                {
                    UserName = username,
                    Email = email,
                    CreatedOn = DateTime.UtcNow,
                    ProfileImagePath = "https://st4.depositphotos.com/1000507/24488/v/600/depositphotos_244889634-stock-illustration-user-profile-picture-isolate-background.jpg"
                };
                if (_userManager is null)
                {
                    return null;
                }
                else
                {
                    try
                    {
                        var result = await _userManager.CreateAsync(user, password);
                        if (result.Succeeded)
                        {
                            var addToROleResult = await _userManager.AddToRoleAsync(user, "User");
                            if (addToROleResult.Succeeded)
                            {
                                _logger.LogInformation($"Added {user.UserName} to role User");
                            }
                            else
                            {
                                _logger.LogError($"Could not add {user.UserName} to role User");
                            }
                            await _signInManager.SignInAsync(user, isPersistent: remember);

                            return new JsonResult(new { success = true });
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.ToString());
                        return new JsonResult(new { exception = e.ToString(), inner = e.InnerException?.ToString() });
                    }
                }

            }
            return new JsonResult(new { success = false });
        }
        #endregion


        #region Login
        public async Task<JsonResult> OnPostLoginAsync(string username, string password, string remember)
        {
            if (ModelState.IsValid)
            {
                //är remembersträngen null är inte kom ihåg mig checkad
                var result = await _signInManager.PasswordSignInAsync(username, password, !string.IsNullOrEmpty(remember), false);

                if (result.Succeeded)
                {
                    var currentUser = await _context.Users.FirstOrDefaultAsync(user => user.UserName == username);
                    _logger.LogInformation("{0} logged in.", currentUser.UserName);
                    return new JsonResult(new { success = true, lockout = false });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return new JsonResult(new { success = false, lockout = true });
                }
                else
                {
                    _logger.LogError("Could not log in");
                    return new JsonResult(new { success = false, lockout = false });
                }
            }

            // If we got this far, something failed, redisplay form
            return new JsonResult(new { success = false, lockout = false });
        }

        #endregion


        #region Create Forum
        public void OnPostCreateForum(string forumName)
        {
            if (_userProfile.IsAdmin && _userProfile.IsLoggedIn)
            {

                var forum = new Forum()
                {
                    Name = forumName
                };
                _context.Forums.Add(forum);
                _context.SaveChanges();
            }
        }
        #endregion


        #region Create subforum
        public void OnPostCreateSubforum(string subName, int parent)
        {
            if (_userProfile.IsAdmin && _userProfile.IsLoggedIn)
            {
                var parentForum = _context.Forums.Where(forum => forum.ID == parent).Include(forum => forum.Subforums).FirstOrDefault();
                var subforum = new Subforum()
                {
                    Name = subName
                };
                parentForum.Subforums.Add(subforum);
                _context.SaveChanges();
            }
        }
        #endregion


        #region Load Messages

        public PartialViewResult OnGetLoadMessages(int id)
        {

            var currentUser = _context.Users.Where(user => user.UserName == _userProfile.Username).FirstOrDefault();
            var chatModel = _context.Chats.Where(chat => chat.ID == id)
                                          .Include(chat => chat.Messages)
                                          .Include(chat => chat.Participant1)
                                          .Include(chat => chat.Participant2)
                                          .AsSplitQuery()
                                          .FirstOrDefault();

            if (chatModel == null || !chatModel.Messages.Any())
            {
                return null;
            }
            chatModel.Messages.Where(message => !message.HasBeenViewed && message.Sender != currentUser.UserName).ToList().ForEach(message => message.HasBeenViewed = true);
            _context.SaveChanges();
            return Partial("_Chat", chatModel);
        }
        #endregion


        #region Send from chat

        public async Task<PartialViewResult> OnPostSendMessageFromChatAsync(string recipient, string title, string message)
        {
            var reciever = await _context.Users.FirstOrDefaultAsync(user => user.UserName == recipient);
            var currentUser = await _context.Users.FirstOrDefaultAsync(user => user.UserName == _userProfile.Username);
            Chat chat = await _context.Chats.Where(chat => chat.Participant1 == reciever && chat.Participant2 == currentUser || chat.Participant2 == reciever && chat.Participant1 == currentUser)
                                            .Include(chat => chat.Messages)
                                            .AsSplitQuery()
                                            .FirstOrDefaultAsync();

            chat.Messages.Add(new()
            {
                MessageTitle = title,
                MessageBody = message,
                DateSent = DateTime.Now,
                Sender = currentUser.UserName

            });

            await _context.SaveChangesAsync();
            return await OnGetLoadNewMessagesAsync(chat.ID, chat.Messages.Count - 1);

        }
        #endregion


        #region Compose
        public PartialViewResult OnGetCompose()
        {
            return Partial("_Compose");
        }

        #endregion


        #region Load new messages
        public async Task<PartialViewResult> OnGetLoadNewMessagesAsync(int chatID, int currentMessagesShown)
        {
            if (_userProfile.IsLoggedIn)
            {

                var model = await _context.Chats.Where(chat => chat.ID == chatID).Include(chat => chat.Messages).FirstOrDefaultAsync();
                model.Messages.Where(message => !message.HasBeenViewed && message.Sender != _userProfile.Username)
                    .ToList()
                    .ForEach(message => message.HasBeenViewed = true);
                await _context.SaveChangesAsync();
                return Partial("_ChatAppendMessages", model.Messages.OrderBy(message => message.DateSent).Skip(currentMessagesShown).ToList());
            }
            else
            {
                return null;
            }
        }
        #endregion


        #region Load message "table"
        public async Task<PartialViewResult> OnGetUpdateMessageTableAsync(int unreadMessages)
        {

            if (!_userProfile.IsLoggedIn)
            {

                return null;
            }
            var model = await _context.Chats.Where(chat => chat.Participant1 == _userProfile.CurrentUser || chat.Participant2 == _userProfile.CurrentUser)
                                  .Include(Chat => Chat.Messages)
                                  .Include(chat => chat.Participant1)
                                  .Include(chat => chat.Participant2)
                                  .AsSplitQuery()
                                  .ToListAsync();
            int newMessages = model.Sum(model => model.Messages.Count(message => !message.HasBeenViewed && message.Sender != _userProfile.Username));
            if (newMessages != unreadMessages)
            {
                return Partial("_MessageTable", model);
            }
            else
            {
                return null;
            }

        }
        #endregion


        #region Update the number near the envelope
        public async Task<JsonResult> OnGetNewMessageCountAsync()
        {

            if (!_userProfile.IsLoggedIn)
            {

                return null;
            }
            var model = await _context.Chats.Where(chat => chat.Participant1 == _userProfile.CurrentUser || chat.Participant2 == _userProfile.CurrentUser)
                                            .Include(Chat => Chat.Messages)
                                            .Include(chat => chat.Participant1)
                                            .Include(chat => chat.Participant2)
                                            .AsSplitQuery()
                                            .ToListAsync();
            int newMessages = model.Sum(model => model.Messages.Count(message => !message.HasBeenViewed && message.Sender != _userProfile.Username));
            return new(new { messages = newMessages > 9 ? "9+" : $"{newMessages}" });



        }
        #endregion


        #region Check if user exists for the indicatior
        public JsonResult OnGetUserExists(string username)
        {
            username = username.ToLower();
            if (_userProfile.Username.ToLower() == username)
            {
                return new JsonResult(new { exists = false });
            }
            bool exists = _context.Users.Any(user => user.UserName.ToLower() == username);
            return new JsonResult(new { exists });
        }
        #endregion

        #region Report
        public async Task<JsonResult> OnPostReportAsync(string type, int id)
        {
            if (_userProfile.IsLoggedIn)
            {
                object reported = type == "reply" ? await _context.Replies.FirstOrDefaultAsync(reply => reply.ID == id) : await _context.Threads.FirstOrDefaultAsync(reply => reply.ID == id);


                Report report = new()
                {
                    Reporter = _userProfile.CurrentUser,
                    ActionTaken = false,
                    Removed = false,
                    ReportedReply = reported is ForumReply ? reported as ForumReply : null,
                    ReportedThread = reported is ForumThread ? reported as ForumThread : null,
                    DateReported = DateTime.Now
                };

                _context.Reports.Add(report);
                int rowsChanged = await _context.SaveChangesAsync();

                return new(new { succeeded = rowsChanged == 1 });
            }

            return new(new { succeeded = false });
        }
        #endregion

        #region Remove forum
        public async Task OnPostRemoveForumAsync(int id)
        {
            if (_userProfile.IsAdmin)
            {
                var forum = await _context.Forums.Where(forum => forum.ID == id)
                                                 .Include(forum => forum.Subforums)
                                                    .ThenInclude(subforum => subforum.Threads)
                                                        .ThenInclude(thread => thread.Replies)
                                                .AsSplitQuery()
                                                .FirstOrDefaultAsync();
                var reports = await _context.Reports.Include(report => report.ReportedReply)
                                                    .Include(report => report.ReportedThread)
                                                    .AsSplitQuery()
                                                    .ToListAsync();
                foreach(var subforum in forum.Subforums)
                {
                    foreach (var thread in subforum.Threads)
                    {
                        foreach (var reply in thread.Replies)
                        {
                            var reportedReply = reports.Where(report => report.ReportedReply == reply).FirstOrDefault();
                            if (reportedReply != null)
                            {
                                _context.Reports.Remove(reportedReply);
                            }
                        }
                        _context.Replies.RemoveRange(thread.Replies);

                        var reprotedThread = reports.Where(report => report.ReportedThread == thread).FirstOrDefault();
                        if (reprotedThread != null)
                        {
                            _context.Reports.Remove(reprotedThread);
                        }
                    }

                    _context.Threads.RemoveRange(subforum.Threads);
                }
                _context.Subforums.RemoveRange(forum.Subforums);
                _context.Forums.Remove(forum);
                await _context.SaveChangesAsync();
            }
        }
        #endregion


        #region Remove subforum
        public async Task OnPostRemoveSubforumAsync(int id)
        {
            if (_userProfile.IsAdmin)
            {
                var subforum = await _context.Subforums.Where(subforum => subforum.ID == id)
                                                 .Include(forum => forum.Threads)
                                                 .ThenInclude(thread => thread.Replies)
                                                 .AsSplitQuery()
                                                 .FirstOrDefaultAsync();

                var reports = await _context.Reports.Include(report => report.ReportedReply)
                                                    .Include(report => report.ReportedThread)
                                                    .AsSplitQuery()
                                                    .ToListAsync();



                foreach (var thread in subforum.Threads)
                {
                    foreach(var reply in thread.Replies)
                    {
                        var reportedReply = reports.Where(report => report.ReportedReply == reply).FirstOrDefault();
                        if(reportedReply != null)
                        {
                            _context.Reports.Remove(reportedReply);
                        }
                    }
                    _context.Replies.RemoveRange(thread.Replies);

                    var reportedThread = reports.Where(report => report.ReportedThread == thread).FirstOrDefault();
                    if (reportedThread != null)
                    {
                        _context.Reports.Remove(reportedThread);
                    }
                }

                _context.Threads.RemoveRange(subforum.Threads);
                _context.Subforums.Remove(subforum);
                await _context.SaveChangesAsync();
            }
        }
        #endregion

    }
}
