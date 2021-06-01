using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnackisDB.Models;
using SnackisDB.Models.Identity;

namespace SnackisForum.Pages
{
    public class AjaxModel : PageModel
    {

        private readonly UserManager<SnackisUser> _userManager;
        private readonly SignInManager<SnackisUser> _signInManager;
        private readonly SnackisContext _context;
        private readonly ILogger<AjaxModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;



        public AjaxModel(UserManager<SnackisUser> userManager, SignInManager<SnackisUser> signInManager, 
                        SnackisContext context, ILogger<AjaxModel> logger, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
        }
        public async Task<JsonResult> OnPostMakeAdminAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var result = await _userManager.AddToRoleAsync(user, "Admin");
            if (result.Succeeded)
            {
                return new JsonResult(new { success = true });
            }
            else
                return new JsonResult(new { success = false });

        }
        public async Task<JsonResult> OnPostLogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return new JsonResult(new { loggedout = true });
        }

        public async Task<JsonResult> OnPostRegisterAsync(string email, string username, string password)
        {

            bool rolesExist = await _roleManager.RoleExistsAsync("Admin");
            if(!rolesExist)
            {
                CreateRoles();
            }
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
                    CreatedOn = DateTime.UtcNow
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
                            await _signInManager.SignInAsync(user, isPersistent: false);

                            return new JsonResult(new { success = true });
                        }
                    }
                    catch (Exception e)
                    {
                        return new JsonResult(new { exception = e.ToString(), inner = e.InnerException.ToString() });
                    }
                }

            }
            return new JsonResult(new { success = false });
        }

        public async Task<JsonResult> OnPostLoginAsync(string username, string password, bool remember)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(username, password, remember, false);

                if (result.Succeeded)
                {
                    var currentUser = await _context.Users.FirstOrDefaultAsync(user => user.UserName == username);
                    _logger.LogInformation("{0} logged in.", currentUser.UserName);
                    return new JsonResult(new { success = true, lockout = false});
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








        private async void CreateRoles()
        {
            IdentityRole[] roles = { new IdentityRole
            {
                Name = "Admin"
            }, new IdentityRole
            {
                Name = "User"
            } 
            };
            foreach(var role in roles)
            { 
            await _roleManager.CreateAsync(role);
            }
        }




















        //public async Task<JsonResult> OnGetRepliesAsync(int id)
        //{
        //    ViewData["ajax"] = true;
        //    // Alternative format of the node (id & parent are required)
        //    //          {
        //    //              id: "string" // required
        //    //parent: "string" // required
        //    //text: "string" // node text
        //    //icon: "string" // string for custom
        //    //state:
        //    //              {
        //    //                  opened: boolean  // is the node open
        //    //              disabled  : boolean  // is the node disabled
        //    //              selected  : boolean  // is the node selected
        //    //},
        //    //li_attr: { }  // attributes for the generated LI node
        //    //              a_attr: { }  // attributes for the generated A node
        //    //          }
        //    //      }

        //    var o = await _context.Threads.Where(thread => thread.ID == id)?
        //                                      .Include(thread => thread.Replies)
        //                                         .ThenInclude(replies => replies.Poster)
        //                                         .AsSingleQuery()
        //                                .Select(thread => thread.Replies.Select(reply =>
        //                                new
        //                                {
        //                                    id = reply.ID.ToString(),
        //                                    parent = reply.ParentComment == null ? "#" : reply.ParentComment.ID.ToString(),
        //                                    text = $"{reply.ReplyTitle} <span style='font-size: 75%'>av {reply.Poster.UserName} {reply.DaysAgo()}</span>" +
        //                                    $"<div class='ms-4'>Visa Svara</div>",
        //                                    state = new
        //                                    {
        //                                        opened = reply.Replies.Any()
        //                                    },
        //                                    icon = "comment"
        //                                })).ToListAsync();
        //    var returnValue = o[0];
        //    return new JsonResult(returnValue);
        //}
        //public async Task<JsonResult> OnGetLoadCommentAsync(int id)
        //{
        //    var comment = await _context.Replies.Where(reply => reply.ID == id)
        //        .Include(reply => reply.Author).FirstOrDefaultAsync();
        //    var returnValue = new { comment = comment.Body, poster = comment.Author.UserName, title = comment.Title, date = comment.DaysAgo()};
        //    return new JsonResult(returnValue);
        //}

        //private string LinkSection(int id)
        //{
        //    return "";
        //}

    }
}
