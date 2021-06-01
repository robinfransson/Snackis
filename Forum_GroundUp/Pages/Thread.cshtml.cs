using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackisDB.Models;
using SnackisDB.Models.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using SnackisForum.Injects;
using Microsoft.Extensions.Logging;

namespace SnackisForum.Pages
{
    public class ThreadModel : PageModel
    {
        public ForumThread Thread { get; set; }


        public bool IsLoggedIn { get; set; }
        public DateTime CreatedOn { get; set; }
        private readonly UserManager<SnackisUser> _userManager;
        private readonly SignInManager<SnackisUser> _signInManager;
        private readonly SnackisContext _context;
        private readonly ILogger<ThreadModel> _logger;

        [BindProperty]
        public ForumReply Reply { get; set; }


        public ThreadModel(UserManager<SnackisUser> userManager, SignInManager<SnackisUser> signInManager,
            SnackisContext context, ILogger<ThreadModel> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }
        public IActionResult OnGet(int id)
        {
            Thread = _context.Threads.Where(thread => thread.ID == id)?
                                        .Include(thread => thread.Replies)
                                            .ThenInclude(replies => replies.Author)
                                            .Include(thread => thread.Parent)
                                     .FirstOrDefault();
            if(Thread is null)
            {
                return RedirectToPage("/");
            }

            CreatedOn = Thread.Replies.OrderBy(reply => reply.DatePosted).First().DatePosted;
            Thread.Replies = Thread.Replies
                                   .OrderBy(reply => reply.DatePosted)
                                   .ToList();
            Thread.Views++;
            _context.SaveChanges();
            IsLoggedIn = _signInManager.IsSignedIn(User);
            return Page();
        }










        public async Task<IActionResult> OnPostAsync(int id)
        {
            Reply.Author = await _userManager.GetUserAsync(User);
            Reply.DatePosted = DateTime.Now;

            Thread = _context.Threads.Include(thread => thread.Replies)
                .Include(thread => thread.Parent).FirstOrDefault(thread => thread.ID == id);
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
