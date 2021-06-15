using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnackisDB.Models;
using SnackisDB.Models.Identity;
using SnackisForum.Injects;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SnackisForum.Pages
{
    public class ThreadModel : PageModel
    {
        private readonly SignInManager<SnackisUser> _signInManager;
        private readonly SnackisContext _context;
        private readonly ILogger<ThreadModel> _logger;

        [BindProperty]
        public ForumReply Reply { get; set; }


        public ThreadModel(SignInManager<SnackisUser> signInManager,
            SnackisContext context, ILogger<ThreadModel> logger)
        {
            _logger = logger;
            _context = context;
            _signInManager = signInManager;
        }




        public ForumThread Thread { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime CreatedOn { get; set; }






        public IActionResult OnGet(int id)
        {
            Thread = _context.Threads.Where(thread => thread.ID == id)?
                                        .Include(thread => thread.CreatedBy)
                                        .Include(thread => thread.Replies)
                                            .ThenInclude(replies => replies.Author)
                                            .Include(thread => thread.Parent)
                                     .FirstOrDefault();
            if (Thread is null)
            {
                return RedirectToPage("/");
            }


            Thread.Replies = Thread.Replies
                                   .OrderBy(reply => reply.DatePosted)
                                   .ToList();
            Thread.Views++;
            _context.SaveChanges();
            IsLoggedIn = _signInManager.IsSignedIn(User);
            return Page();
        }










        public async Task<IActionResult> OnPostAsync(int id, [FromServices] UserProfile profile)
        {
            Reply.Author = profile.IsLoggedIn ? profile.CurrentUser : null;
            Reply.DatePosted = DateTime.Now;

            Thread = _context.Threads.Where(thread => thread.ID == id)
                                     .Include(thread => thread.Replies)
                                     .FirstOrDefault();
            Thread.Replies.Add(Reply);
            int changes = await _context.SaveChangesAsync();
            _logger.LogInformation(changes + " rows changed");
            return RedirectToPage();
        }


















        public JsonResult OnPostGoToLastReply(int postID, int threadID)
        {

            Thread = _context.Threads.Where(thread => thread.ID == threadID)?
                                        .Include(thread => thread.Replies)

                                     .FirstOrDefault();
            double indexOfReply = Thread.Replies.IndexOf(Thread.Replies.FirstOrDefault(reply => reply.ID == postID));

            double page = Math.Ceiling(indexOfReply / 10d);

            return new JsonResult(new { threadID, page, reply = indexOfReply + 1 });
        }
    }
}
