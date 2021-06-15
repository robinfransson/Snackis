using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SnackisDB.Models;
using SnackisForum.Injects;
using System.Collections.Generic;
using System.Linq;

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
        public IActionResult OnGet([FromServices] UserProfile profile)
        {
            if (profile.IsAdmin)
            {
                Forums = _context.Forums.Include(forum => forum.Subforums)
                                            .ThenInclude(sub => sub.Threads)
                                                .ThenInclude(thread => thread.Replies)
                                            .AsSplitQuery()
                                            .ToList();
                Reports = _context.Reports.Count(report => !report.ActionTaken);
                Users = _context.Users.Count();
                return Page();
            }
            return RedirectToPage("../Index");

        }
    }
}
