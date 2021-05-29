using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SnackisDB.Models;
using SnackisDB.Models.Identity;
using SnackisForum.Injects;

namespace Chatt_test.Pages
{
    public class IndexModel : PageModel
    {
        public bool LoggedIn { get; set; }
        public List<Forum> Forums { get; set; }

        private readonly SignInManager<SnackisUser> _signInManager;
        public readonly UserProfile _profile;

        public IndexModel(SignInManager<SnackisUser> signInManager, UserProfile profile)
        {
            _signInManager = signInManager;
            _profile = profile;
        }
        public void OnGet([FromServices] SnackisContext context)
        {
            Forums = context.Forums.Include(forum => forum.Subforums)
                                        .ThenInclude(sub => sub.LastReply)
                                            .ThenInclude(reply => reply.Poster)
                                   .Include(forum => forum.Subforums)
                                        .ThenInclude(sub => sub.Threads)
                                            .ThenInclude(thread => thread.Replies)
                                   .ToList();
            LoggedIn = _signInManager.IsSignedIn(User);
        }


    }
}
