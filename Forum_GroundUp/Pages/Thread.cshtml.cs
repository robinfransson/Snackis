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



        public ThreadModel(UserManager<SnackisUser> userManager, SignInManager<SnackisUser> signInManager, SnackisContext context)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }
        public void OnGet(int id, int sheet)
        {
            Thread = _context.Threads.Where(thread => thread.ID == id)?
                                        .Include(thread => thread.Replies)
                                            .ThenInclude(replies => replies.Poster)

                                     .FirstOrDefault();
            CreatedOn = Thread.Replies.OrderBy(reply => reply.DatePosted).First().DatePosted;
            Thread.Replies = Thread.Replies
                                   .OrderBy(reply => reply.DatePosted)
                                   .Skip(sheet * 10)
                                   .Take(10)
                                   .ToList();

            IsLoggedIn = _signInManager.IsSignedIn(User);
        }


        public async Task<JsonResult> OnGetRepliesAsync(int id)
        {
            ViewData["ajax"] = true;
            // Alternative format of the node (id & parent are required)
            //          {
            //              id: "string" // required
            //parent: "string" // required
            //text: "string" // node text
            //icon: "string" // string for custom
            //state:
            //              {
            //                  opened: boolean  // is the node open
            //              disabled  : boolean  // is the node disabled
            //              selected  : boolean  // is the node selected
            //},
            //li_attr: { }  // attributes for the generated LI node
            //              a_attr: { }  // attributes for the generated A node
            //          }
            //      }

            var o = await _context.Threads.Where(thread => thread.ID == id)?
                                              .Include(thread => thread.Replies)
                                                 .ThenInclude(replies => replies.Poster)
                                        .Select(thread => thread.Replies.Select(reply =>
                                        new
                                        {
                                            id = reply.ID.ToString(),
                                            parent = reply.ParentComment == null ? "#" : reply.ParentComment.ID.ToString(),
                                            text = $"{reply.ReplyTitle} <div style='font-size: 75%'>av {reply.Poster.UserName}",
                                            state = new { opened = true, selected = true },
                                            icon = "comment"

                                        }))
                                        .ToListAsync();
            return new JsonResult(o);
        }
    }
}
