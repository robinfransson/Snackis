using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackisForum.Injects;
using SnackisDB.Models.Identity;
using SnackisDB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace SnackisForum.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly SnackisContext _context;



        public IndexModel(SnackisContext context)
        {
            _context = context;
        }




        public List<Forum> Forums { get; set; }
        public int Users { get; set; }
        public int Reports { get; set; }
        public IActionResult OnGet()
        {
            Forums = _context.Forums.Include(forum => forum.Subforums).ThenInclude(sub => sub.Threads).ThenInclude(thread => thread.Replies).ToList();
            Reports = _context.Reports.Count(report => !report.ActionTaken);
            Users = _context.Users.Count();
            return Page();

        }
    }
}
