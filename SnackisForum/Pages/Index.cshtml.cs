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

namespace Chatt_test.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class IndexModel : PageModel
    {
        public bool LoggedIn { get; set; }
        public List<Forum> Forums { get; set; }



        private readonly SignInManager<SnackisUser> _signInManager;
        public readonly UserProfile _profile;
        public readonly SnackisContext _context;
        public readonly ILogger<IndexModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IndexModel(SignInManager<SnackisUser> signInManager, UserProfile profile, 
                          SnackisContext context, ILogger<IndexModel> logger,
                          RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _profile = profile;
            _context = context;
            _logger = logger;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!_context.Database.CanConnect())
            {

                return await InitialSetup();
            }
            Forums = await _context.Forums.Include(forum => forum.Subforums)
                                    .ThenInclude(sub => sub.Threads)
                                        .ThenInclude(thread => thread.Replies)
                                        .ThenInclude(reply => reply.Author)
                               .ToListAsync();

            return Page();

        }

        public async Task<IActionResult> InitialSetup()
        {
            _logger.LogInformation("Setting up the database...");
            _context.Database.EnsureCreated();
            bool rolesExist = await _roleManager.RoleExistsAsync("Admin");
            if (!rolesExist)
            {
                await CreateRoles();
            }
            _logger.LogInformation("Done!");
            return RedirectToPage();
        }

        #region Create roles

        private async Task CreateRoles()
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
                _logger.LogInformation("Created role " + role.Name);
                await _roleManager.CreateAsync(role);
            }
        }

        #endregion
    }
}
