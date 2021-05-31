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


        public int PageNumber { get; set; }
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
        public IActionResult OnGet(int id, int pageNumber)
        {
            if(pageNumber > 0)
            {
                pageNumber--;
            }
            Thread = _context.Threads.Where(thread => thread.ID == id)?
                                        .Include(thread => thread.Replies)
                                            .ThenInclude(replies => replies.Author)

                                     .FirstOrDefault();
            double maxPageInteger = Math.Ceiling(Thread.Replies.Count / 10d);

            CreatedOn = Thread.Replies.OrderBy(reply => reply.DatePosted).First().DatePosted;
            if(!Thread.Replies.Any() || pageNumber > maxPageInteger)
            {
                return RedirectToPage("Thread", new { id, pageNumber = maxPageInteger });
            }
            Thread.Replies = Thread.Replies
                                   .OrderBy(reply => reply.DatePosted)
                                   .Skip(pageNumber * 10)
                                   .Take(10)
                                   .ToList();
            PageNumber = pageNumber;
            IsLoggedIn = _signInManager.IsSignedIn(User);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            Reply.Author = await _userManager.GetUserAsync(User);
            Reply.DatePosted = DateTime.Now;

            _context.Threads.Include(thread => thread.Replies).FirstOrDefault(thread => thread.ID == id).Replies.Add(Reply);
            int changes = await _context.SaveChangesAsync();
            _logger.LogInformation(changes + " rows changed");
            return RedirectToPage();
        }
        //public async Task<JsonResult> OnGetRepliesAsync(int id)
        //{
        //    ViewData["ajax"] = true;
        //    // Alternative format of the node (id & parent are required)
        //    //          {
        //    //              id: "string" // required
        //    //parent: "string" // required
        //    //text: "string" // node text
        //    //icon: "string" // string for custom
        //    //state:
        //    //              {
        //    //                  opened: boolean  // is the node open
        //    //              disabled  : boolean  // is the node disabled
        //    //              selected  : boolean  // is the node selected
        //    //},
        //    //li_attr: { }  // attributes for the generated LI node
        //    //              a_attr: { }  // attributes for the generated A node
        //    //          }
        //    //      }

        //    var o = await _context.Threads.Where(thread => thread.ID == id)?
        //                                      .Include(thread => thread.Replies)
        //                                         .ThenInclude(replies => replies.Author)
        //                                .Select(thread => thread.Replies.Select(reply =>
        //                                new
        //                                {
        //                                    id = reply.ID.ToString(),
        //                                    parent = reply. == null ? "#" : reply.ParentComment.ID.ToString(),
        //                                    text = $"{reply.ReplyTitle} <div style='font-size: 75%'>av {reply.Poster.UserName}",
        //                                    state = new { opened = true, selected = true },
        //                                    icon = "comment"

        //                                }))
        //                                .ToListAsync();
        //    return new JsonResult(o);
        //}
    }
}
