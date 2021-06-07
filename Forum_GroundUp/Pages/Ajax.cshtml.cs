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
using SnackisForum.Injects;

namespace SnackisForum.Pages
{
    public class AjaxModel : PageModel
    {
        #region Constructor and readonlies
        private readonly UserManager<SnackisUser> _userManager;
        private readonly SignInManager<SnackisUser> _signInManager;
        private readonly SnackisContext _context;
        private readonly ILogger<AjaxModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserProfile _userProfile;



        public AjaxModel(UserManager<SnackisUser> userManager, SignInManager<SnackisUser> signInManager,
                        SnackisContext context, ILogger<AjaxModel> logger, RoleManager<IdentityRole> roleManager, UserProfile userProfile)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
            _userProfile = userProfile;
        }
        #endregion


        #region Make admin
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

        #endregion


        #region Logout
        public async Task<JsonResult> OnPostLogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return new JsonResult(new { loggedout = true });
        }


        #endregion


        #region Register
        public async Task<JsonResult> OnPostRegisterAsync(string email, string username, string password)
        {

            bool rolesExist = await _roleManager.RoleExistsAsync("Admin");
            if (!rolesExist)
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
        #endregion


        #region Login
        public async Task<JsonResult> OnPostLoginAsync(string username, string password, bool remember)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(username, password, remember, false);

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


        #region Create roles

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
            foreach (var role in roles)
            {
                await _roleManager.CreateAsync(role);
            }
        }

        #endregion

        #region Create Forum
        public void OnPostCreateForum(string forumName)
        {
            if(_userProfile.IsAdmin && _userProfile.IsLoggedIn)
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
    }
}
