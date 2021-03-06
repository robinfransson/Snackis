using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SnackisDB.Models.Identity;
using System.Linq;

namespace SnackisForum.Injects
{
    public class UserProfile
    {
        private readonly UserManager<SnackisDB.Models.Identity.SnackisUser> _userManager;
        private readonly SnackisDB.Models.SnackisContext _context;
        private readonly SignInManager<SnackisUser> _signInManager;
        private HttpContext httpContext;
        public int UnreadMessages
        {
            get
            {
                return !_context.Chats.Any(chat => chat.Participant1 == CurrentUser || chat.Participant2 == CurrentUser) ? 0 :
                                          _context.Chats
                                          .Where(chat => chat.Participant1 == CurrentUser || chat.Participant2 == CurrentUser)
                                          .Include(chat => chat.Messages)
                                          .AsSplitQuery()
                                          .AsEnumerable()
                                          .Sum(chat => chat.Messages.Count(message => !message.HasBeenViewed && message.Sender != Username));

            }
        }

        public SnackisUser CurrentUser => _userManager.GetUserAsync(httpContext.User).Result;

        public string Username => _userManager.GetUserAsync(httpContext.User).Result.UserName;

        public string UserID => _userManager.GetUserId(httpContext.User);
        public string ProfilePicture => _userManager.GetUserAsync(httpContext.User).Result.ProfileImagePath;

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
