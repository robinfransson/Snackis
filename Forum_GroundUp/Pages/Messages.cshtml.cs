using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnackisDB.Models;
using SnackisForum.Injects;
namespace SnackisForum.Pages
{
    public class MessagesModel : PageModel
    {

        #region Readonlies and constuctor
        private readonly SnackisContext _context;
        private readonly UserProfile _profile;
        private readonly ILogger<MessagesModel> _logger;
        public MessagesModel(SnackisContext context, UserProfile userProfile, ILogger<MessagesModel> logger)
        {
            _context = context;
            _profile = userProfile;
            _logger = logger;
        }

        public List<Chat> Chats { get; set; }
        #endregion

        #region On get
        public void OnGet()
        {
            Chats = _context.Chats.Where(chat => chat.Participant1 == _profile.CurrentUser || chat.Participant2 == _profile.CurrentUser)
                                  .Include(Chat => Chat.Messages)
                                  .Include(chat => chat.Participant1)
                                  .Include(chat => chat.Participant2)
                                  .ToList();
            

        }
        #endregion

        #region Load Messages

        public PartialViewResult OnGetLoadMessages(int id)
        {

            var currentUser = _context.Users.Where(user => user.UserName == _profile.Username).FirstOrDefault();
            var chatModel = _context.Chats.Where(chat => chat.ID == id)
                                          .Include(chat => chat.Messages)
                                          .Include(chat => chat.Participant1)
                                          .Include(chat => chat.Participant2)
                                          .FirstOrDefault();
                
            if(chatModel == null || !chatModel.Messages.Any())
            {
                return null;
            }
            chatModel.Messages.Where(message => !message.HasBeenViewed && message.Sender != currentUser.UserName).ToList().ForEach(message => message.HasBeenViewed = true);
            _context.SaveChanges();
            return Partial("_Chat", chatModel);
        }
        #endregion


        #region Check if user exists for the indicatior
        public JsonResult OnGetUserExists(string username)
        {
            username = username.ToLower();
            if (_profile.Username.ToLower() == username)
            {
                return new JsonResult(new { exists = false });
            }
            bool exists = _context.Users.Any(user => user.UserName.ToLower() == username);
            return new JsonResult(new { exists });
        }
        #endregion


        #region Send message
        public IActionResult OnPost(string recipient, string title, string message)
        {
            var reciever = _context.Users.FirstOrDefault(user => user.UserName == recipient);
            var currentUser = _context.Users.FirstOrDefault(user => user.UserName == _profile.Username);
            Chat chat = _context.Chats.Where(chat => chat.Participant1 == reciever && chat.Participant2 == currentUser || chat.Participant2 == reciever && chat.Participant1 == currentUser)
                                      .Include(chat => chat.Messages).FirstOrDefault();
            
            if(chat is null)
            {
                chat = new()
                {
                    Messages = new(),
                    Participant1 = currentUser,
                    Participant2 = reciever
                };

                _context.Chats.Add(chat);
            }

            chat.Messages.Add(new()
            {
                MessageTitle = title,
                MessageBody = message,
                DateSent = DateTime.Now,
                Sender = currentUser.UserName
                
            });

            _context.SaveChanges();
            return RedirectToPage();

        }

        #endregion

        #region Send from chat

        public async Task<PartialViewResult> OnPostSendMessageFromChat(string recipient, string title, string message)
        {
            var reciever = await _context.Users.FirstOrDefaultAsync(user => user.UserName == recipient);
            var currentUser = await _context.Users.FirstOrDefaultAsync(user => user.UserName == _profile.Username);
            Chat chat = await _context.Chats.Where(chat => chat.Participant1 == reciever && chat.Participant2 == currentUser || chat.Participant2 == reciever && chat.Participant1 == currentUser)
                                      .Include(chat => chat.Messages).FirstOrDefaultAsync();

            chat.Messages.Add(new()
            {
                MessageTitle = title,
                MessageBody = message,
                DateSent = DateTime.Now,
                Sender = currentUser.UserName

            });

            await _context.SaveChangesAsync();
            return await OnGetLoadNewMessages(chat.ID, chat.Messages.Count-1);

        }
        #endregion

        #region Compose
        public PartialViewResult OnGetCompose()
        {
            return Partial("_Compose");
        }

        #endregion

        #region Load new messages
        public async Task<PartialViewResult> OnGetLoadNewMessages(int chatID, int currentMessagesShown)
        {
            var model = await _context.Chats.Where(chat => chat.ID == chatID).Include(chat => chat.Messages).FirstOrDefaultAsync();
            model.Messages.Where(message => !message.HasBeenViewed && message.Sender != _profile.Username).ToList().ForEach(message => message.HasBeenViewed = true);

            return Partial("_ChatAppendMessages", model.Messages.OrderBy(message => message.DateSent).Skip(currentMessagesShown).ToList());
        }
        #endregion
    }
}
