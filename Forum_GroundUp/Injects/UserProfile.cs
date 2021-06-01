using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SnackisDB.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnackisForum.Injects
{
    public class UserProfile
    {
        private readonly UserManager<SnackisDB.Models.Identity.SnackisUser> _userManager;
        private readonly SnackisDB.Models.SnackisContext _context;
        private readonly SignInManager<SnackisUser> _signInManager;
        private HttpContext httpContext;
        public int UnreadMessages { 
            get {
                return _context.Messages.Count(message => message.RecieverID == _userManager.GetUserId(httpContext.User) && !message.HasBeenViewed);
                }
        }
        public string Username => _userManager.GetUserAsync(httpContext.User).Result.UserName;

        public string UserID => _userManager.GetUserId(httpContext.User);

        public bool IsAdmin => _userManager.IsInRoleAsync(_userManager.GetUserAsync(httpContext.User).Result, "Admin").Result;

        public int Notifications { get; set; }
        //public int Notifications { get; set; }
        public bool IsLoggedIn => _signInManager.IsSignedIn(httpContext.User);
        public UserProfile(UserManager<SnackisDB.Models.Identity.SnackisUser> userManager, 
                           IHttpContextAccessor httpContextAccessor, SnackisDB.Models.SnackisContext context,
                           SignInManager<SnackisUser> signInManager)
        {
            _userManager = userManager;
            httpContext = httpContextAccessor.HttpContext;
            _context = context;
            _signInManager = signInManager;
        }
    }
}
