using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnackisDB.Models;
using SnackisForum.Injects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chatt_test.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class IndexModel : PageModel
    {


        public readonly UserProfile _profile;
        public readonly SnackisContext _context;
        public readonly ILogger<IndexModel> _logger;

        public IndexModel(UserProfile profile, SnackisContext context, ILogger<IndexModel> logger)
        {
            _profile = profile;
            _context = context;
            _logger = logger;
        }


        public bool LoggedIn { get; set; }
        public List<Forum> Forums { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {

            Forums = await _context.Forums.Include(forum => forum.Subforums)
                                    .ThenInclude(sub => sub.Threads)
                                        .ThenInclude(thread => thread.Replies)
                                            .ThenInclude(reply => reply.Author)
                                    .Include(forum => forum.Subforums)
                                    .ThenInclude(sub => sub.Threads)
                                    .ThenInclude(thread => thread.CreatedBy)
                                    .AsSplitQuery()
                               .ToListAsync();

            return Page();

        }

    }
}
