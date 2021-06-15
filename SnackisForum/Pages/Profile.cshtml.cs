using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SnackisDB.Models;
using SnackisDB.Models.Identity;
using SnackisForum.Injects;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SnackisForum.Pages
{
    [Authorize(Roles = "User,Admin")]
    public class ProfileModel : PageModel
    {
        private readonly UserManager<SnackisUser> _userManager;
        private readonly SignInManager<SnackisUser> _signInManager;
        private readonly SnackisContext _context;
        private readonly ILogger<ProfileModel> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;





        public ProfileModel(UserManager<SnackisUser> userManager, SignInManager<SnackisUser> signInManager,
                            SnackisContext context, ILogger<ProfileModel> logger, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _hostingEnvironment = hostingEnvironment;
        }


        [BindProperty]
        public IFormFile ProfilePicture { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost([FromServices] UserProfile userProfile)
        {
            string wwwPath = this._hostingEnvironment.WebRootPath + "/images/";
            var file = wwwPath + ProfilePicture.FileName;
            using (var fileStream = new FileStream(file, FileMode.Create))
            {
                await ProfilePicture.CopyToAsync(fileStream);
            }

            _context.Users.First(user => user.Id == userProfile.UserID).ProfileImagePath = "/images/" + ProfilePicture.FileName;
            await _context.SaveChangesAsync();

            return Page();
        }
    }
}
