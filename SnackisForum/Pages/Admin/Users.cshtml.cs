using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnackisDB.Models;
using SnackisDB.Models.Identity;
using SnackisForum.Injects;

namespace SnackisForum.Pages.Admin
{
    public class UsersModel : PageModel
    {
        private readonly SnackisContext _context;
        private readonly ILogger<UsersModel> _logger;
        private readonly UserProfile _profile;



        public UsersModel(SnackisContext context, ILogger<UsersModel> logger, UserProfile userProfile)
        {
            _context = context;
            _logger = logger;
            _profile = userProfile;
        }


        public List<SnackisUser> Users { get; set; }
        public int Reports { get; set; }


        public IActionResult OnGet()
        {
            if (_profile.IsAdmin)
            {

                Users = _context.Users.ToList();
                Reports = _context.Reports.Count(report => !report.ActionTaken);
                return Page();
            }
            return RedirectToPage("../Index");
        }
    }
}
