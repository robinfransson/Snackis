using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SnackisDB.Models;
using SnackisDB.Models.Identity;

namespace SnackisForum.Pages
{
    public class AccountModel : PageModel
    {
        private readonly SignInManager<SnackisUser> _signInManager;
        private readonly UserManager<SnackisUser> _userManager;
        private readonly ILogger<AccountModel> _logger;
        private readonly SnackisContext _context;

        public AccountModel(SignInManager<SnackisUser> signInManager, UserManager<SnackisUser> userManager, 
                            ILogger<AccountModel> logger, SnackisContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }
        public void OnGet()
        {
        }



        //public async Task<List<Message>> OnPostGetMessagesAsync()
        //{
        //    string userID = _userManager.GetUserId(User);
        //    return _context.Messages.Where(message => message.UserID == userID).ToList();
        //}


        public async Task<JsonResult> OnPostLogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return new JsonResult(new { loggedout = true });
        }

            public async Task<JsonResult> OnPostRegisterAsync(string email,string username, string password)
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
                    Email = email
                };
                if(_userManager is null)
                {
                    return null;
                }
                else
                {
                    try { 
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return new JsonResult(new { success = true });
                }
                    }
                    catch(Exception e)
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
                    _logger.LogInformation("{0} ({1}) logged in.", currentUser.UserName, currentUser.ForumUserID);
                    return new JsonResult(new { success = true, lockout = false, userID = currentUser.ForumUserID});
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
    }
}
