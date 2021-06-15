using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SnackisDB.Models;
using SnackisDB.Models.Identity;
using System.Collections.Generic;
using System.Linq;

namespace SnackisForum.Pages
{
    public class SubforumModel : PageModel
    {

        public List<ForumThread> Threads { get; set; }

        public Subforum Subforum { get; set; }


        private readonly UserManager<SnackisUser> _userManager;
        private readonly SignInManager<SnackisUser> _signInManager;
        private readonly SnackisContext _context;



        public SubforumModel(UserManager<SnackisUser> userManager, SignInManager<SnackisUser> signInManager, SnackisContext context)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
        }
        public void OnGet(int id)
        {
            Subforum = _context.Subforums.Where(subforum => subforum.ID == id)?.Include(sub => sub.Threads)
                                                                                   .ThenInclude(thread => thread.Replies)
                                                                                       .ThenInclude(thread => thread.Author)
                                                                                .Include(sub => sub.Threads)
                                                                                        .ThenInclude(thread => thread.CreatedBy)
                                                                                .FirstOrDefault();
        }



    }
}
