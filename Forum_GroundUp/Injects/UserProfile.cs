using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
        private HttpContext httpContext;
        public int UnreadMessages { 
            get {
                return _context.Messages.Count(message => message.RecieverID == _userManager.GetUserId(httpContext.User) && !message.HasBeenViewed);
                }
        }

        public string UserID => _userManager.GetUserId(httpContext.User);
        public int Notifications { get; set; }
        //public int Notifications { get; set; }

        public UserProfile(UserManager<SnackisDB.Models.Identity.SnackisUser> userManager, IHttpContextAccessor httpContextAccessor, SnackisDB.Models.SnackisContext context)
        {
            _userManager = userManager;
            httpContext = httpContextAccessor.HttpContext;
            _context = context;
        }
    }
}
