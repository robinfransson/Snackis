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

namespace SnackisForum.Pages
{
    public class TreeViewModel : PageModel
    {

        private readonly UserManager<SnackisUser> _userManager;
        private readonly SignInManager<SnackisUser> _signInManager;
        private readonly SnackisContext _context;
        private readonly ILogger<ThreadModel> _logger;



        public TreeViewModel(UserManager<SnackisUser> userManager, SignInManager<SnackisUser> signInManager,
    SnackisContext context, ILogger<ThreadModel> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }
        public void OnGet()
        {

        }
        public async Task<JsonResult> OnGetLoadForumAsync()
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

            List<Forum> Forum = await _context.Forums.Include(forum => forum.Subforums)

                                              .ThenInclude(subforum => subforum.Threads)
                                              .ThenInclude(thread => thread.Replies)
                                                 .ThenInclude(replies => replies.Author)
                                                 .AsSingleQuery().ToListAsync();
            var forums = Forum.Select(forum => new
            {
                id = (forum.ID + 200000).ToString(),
                parent = "#",
                text = forum.Name,
                state = new
                {
                    opened = true
                },
                a_attr = new
                {
                    @class = "forum-tree"
                },
                icon = "forum",
            }).ToList();

            var subforums = Forum.Select(forum => forum.Subforums.Select(sub => new
            {
                id = (sub.ID + 500000).ToString(),
                parent = (forum.ID + 200000).ToString(),
                text = sub.Name,
                state = new
                {
                    opened = false
                },
                a_attr = new
                {
                    @class = "sub-tree"
                },
                icon = "forum",

            })).FirstOrDefault();
            forums.AddRange(subforums);

            var threads = Forum.SelectMany(forum =>
                                       forum.Subforums.SelectMany(
                                             sub => sub.Threads.Select(
                                                        thread => new
                                                        {
                                                            id = (thread.ID + 1000000).ToString(),
                                                            parent = (sub.ID + 500000).ToString(),
                                                            text = thread.Title,
                                                            state = new
                                                            {
                                                                opened = false
                                                            },
                                                            a_attr = new
                                                            {
                                                                @class = "thread-tree"
                                                                
                                                            },
                                                            icon = "message"
                                                        }
                                                   )
                                             )
                                       );
            var replies = Forum.Select(forum => forum.Subforums.SelectMany(sub => sub.Threads.SelectMany(thread => thread.Replies.Select(reply => new
            {
                id = (reply.ID + 10000000).ToString(),
                parent = (thread.ID + 1000000).ToString(),
                text = reply.Title,

                state = new
                {
                    opened = false
                },
                a_attr = new
                {
                    @class = "reply-tree"
                },
                icon = "message"
            })))).FirstOrDefault();
            forums.AddRange(threads);
            forums.AddRange(replies);
            return new JsonResult(forums);
        }



        public async Task<JsonResult> OnGetLoadCommentAsync(string id)
        {
            int realID = 0;
            int.TryParse(id, out realID);
            realID -= 10000000;
            var comment = await _context.Replies.Where(reply => reply.ID == realID)
                .Include(reply => reply.Author).FirstOrDefaultAsync();
            var returnValue = new { comment = comment.Body, poster = comment.Author.UserName, title = comment.Title, date = comment.DaysAgo() };
            return new JsonResult(returnValue);
        }

    }
}
