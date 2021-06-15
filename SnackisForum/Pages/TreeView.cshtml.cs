using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnackisDB.Models;
using SnackisDB.Models.Identity;
using SnackisForum.Injects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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



        [BindProperty]
        public ForumReply Reply { get; set; }


        [BindProperty]
        public ForumThread Thread { get; set; }



        #region Spaghettikod för att returnera forumet i formatet som JSTree vill ha
        public async Task<JsonResult> OnGetLoadForumAsync()
        {
            ViewData["ajax"] = true;
            // Alternative format of the node (id & parent are required)
            //          {
            //              id: "string" // required
            //parent: "string" # == Top- node , else parents id == place under // required
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
                id = "forum-" + forum.ID,
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

            var subforums = Forum.SelectMany(forum => forum.Subforums.Select(sub => new
            {
                id = "subforum-" + sub.ID,
                parent = "forum-" + forum.ID,
                text = sub.Name,
                state = new
                {
                    opened = false
                },
                a_attr = new
                {
                    @class = "sub-tree"
                },
                icon = "textsms",

            })).ToList();
            forums.AddRange(subforums);

            var threads = Forum.SelectMany(forum => forum.Subforums.SelectMany(
                                                    sub => sub.Threads.Select(
                                                        thread => new
                                                        {
                                                            id = "thread-" + thread.ID.ToString(),
                                                            parent = "subforum-" + sub.ID,
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
            var replies = Forum.SelectMany(forum => forum.Subforums.SelectMany(
                                                sub => sub.Threads.SelectMany(
                                                thread => thread.Replies.Select(
                                                reply => new
                                                {

                                                    id = "reply-" + reply.ID,
                                                    parent = reply.RepliedComment == null ? "thread-" + thread.ID : "reply-" + reply.RepliedComment.ID,
                                                    text = string.IsNullOrWhiteSpace(reply.Title) ? "Svar till " + thread.Title : reply.Title,

                                                    state = new
                                                    {
                                                        opened = false
                                                    },
                                                    a_attr = new
                                                    {
                                                        @class = "reply-tree"
                                                    },
                                                    icon = "comment"
                                                })))).ToList();
            forums.AddRange(threads);
            forums.AddRange(replies);
            return new JsonResult(forums);
        }
        #endregion


        #region Öppnar de noder som länkar dit användaren gjorde ett inlägg
        public async Task<JsonResult> OnGetNewEntryAsync(string nodeID)
        {
            // Alternative format of the node (id & parent are required)
            //          {
            //              id: "string" // required
            //parent: "string" # == Top- node , else parents id == place under // required
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
                id = "forum-" + forum.ID,
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

            var subforums = Forum.SelectMany(forum => forum.Subforums.Select(sub => new
            {
                id = "subforum-" + sub.ID,
                parent = "forum-" + forum.ID,
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

            })).ToList();
            forums.AddRange(subforums);

            var threads = Forum.SelectMany(forum => forum.Subforums.SelectMany(
                                                    sub => sub.Threads.Select(
                                                        thread => new
                                                        {
                                                            id = "thread-" + thread.ID.ToString(),
                                                            parent = "subforum-" + sub.ID,
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
            var replies = Forum.SelectMany(forum => forum.Subforums.SelectMany(
                                                sub => sub.Threads.SelectMany(
                                                thread => thread.Replies.Select(
                                                reply => new
                                                {

                                                    id = "reply-" + reply.ID,
                                                    parent = reply.RepliedComment == null ? "thread-" + thread.ID : "reply-" + reply.RepliedComment.ID,
                                                    text = string.IsNullOrWhiteSpace(reply.Title) ? "Svar till " + thread.Title : reply.Title,

                                                    state = new
                                                    {
                                                        opened = false
                                                    },
                                                    a_attr = new
                                                    {
                                                        @class = "reply-tree"
                                                    },
                                                    icon = "message"
                                                })))).ToList();
            forums.AddRange(threads);
            forums.AddRange(replies);


            while (!nodeID.StartsWith("forum"))
            {
                var current = forums.Where(obj => obj.id == nodeID).FirstOrDefault();
                forums.Remove(current);
                forums.Add(new
                {
                    id = nodeID,
                    parent = current.parent,
                    text = current.text,

                    state = new
                    {
                        opened = true
                    },
                    a_attr = current.a_attr,
                    icon = current.icon
                });
                nodeID = current.parent;
            }
            return new JsonResult(forums);
        }
        #endregion


        #region PostThreadReply
        public async Task<IActionResult> OnPostThreadReplyAsync(int threadID, int? repliedCommentID, [FromServices] UserProfile userProfile)
        {
            SnackisUser user = null;
            if (userProfile.IsLoggedIn)
            {

                user = await _userManager.GetUserAsync(User);
            }
            Reply.Author = user;
            Reply.DatePosted = DateTime.Now;
            Reply.RepliedComment = await _context.Replies.FirstOrDefaultAsync(reply => reply.ID == repliedCommentID);

            var thread = _context.Threads.Where(thread => thread.ID == threadID).
                Include(thread => thread.Replies)
                .Include(thread => thread.Parent)
                .FirstOrDefault();
            thread.Replies.Add(Reply);
            int changes = await _context.SaveChangesAsync();
            _logger.LogInformation(changes + " rows changed");
            return RedirectToPage("/treeview", new { nodeID = "reply-" + Reply.ID });
        }
        #endregion


        #region CreateThread
        public async Task<IActionResult> OnPostCreateThreadAsync(int subID, [FromServices] UserProfile userProfile)
        {
            if (!userProfile.IsLoggedIn)
            {
                return Page();
            }
            Thread.CreatedBy = await _userManager.GetUserAsync(User);
            Thread.CreatedOn = DateTime.Now;

            var subforum = _context.Subforums.Where(subforum => subforum.ID == subID).Include(sub => sub.Threads).FirstOrDefault();
            subforum.Threads.Add(Thread);
            int changes = await _context.SaveChangesAsync();
            _logger.LogInformation($"{Thread.CreatedOn:ddMMyy HH:mm}:{changes} rows changed, added new thread ({Thread.Title}) to {subforum.Name} by {Thread.CreatedBy.UserName}");

            return RedirectToPage("/treeview", new { nodeID = "thread-" + Thread.ID });
        }

        #endregion


        #region LoadComment

        public async Task<PartialViewResult> OnGetLoadCommentAsync(string id)
        {
            _ = int.TryParse(id, out int realID);
            var comment = await _context.Replies.Where(reply => reply.ID == realID)
                .Include(reply => reply.Author)
                .Include(reply => reply.Thread)
                .Include(reply => reply.RepliedComment)
                .FirstOrDefaultAsync();
            return Partial("_ReplyModal", comment);
        }

        #endregion


        #region LoadSubforum

        public async Task<PartialViewResult> OnGetLoadSubforumAsync(string id)
        {
            _ = int.TryParse(id, out int realID);
            var subforum = await _context.Subforums.Where(sub => sub.ID == realID)
                .FirstOrDefaultAsync();
            return Partial("_ReplyModal", subforum);
        }

        #endregion


        #region Load Thread reply


        public async Task<PartialViewResult> OnGetLoadThreadAsync(string id)
        {
            _ = int.TryParse(id, out int realID);
            var thread = await _context.Threads.Where(thread => thread.ID == realID)
                .Include(thread => thread.CreatedBy).FirstOrDefaultAsync();

            return Partial("_ReplyModal", thread);
        }
        #endregion

    }
}
