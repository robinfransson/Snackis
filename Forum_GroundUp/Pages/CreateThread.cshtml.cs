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
using SnackisForum.Injects;

namespace SnackisForum.Pages
{
    public class CreateThreadModel : PageModel
    {


        public Subforum Subforum { get; set; }
        [BindProperty]
        public ForumThread Thread { get; set; }
        [BindProperty]
        public ForumReply Reply { get; set; }

        private readonly UserManager<SnackisUser> _userManager;
        private readonly SignInManager<SnackisUser> _signInManager;
        private readonly SnackisContext _context;
        private readonly ILogger<AjaxModel> _logger;


        public CreateThreadModel(UserManager<SnackisUser> userManager, SignInManager<SnackisUser> signInManager, 
            SnackisContext context, ILogger<AjaxModel> logger)
        {
            _userManager = userManager;
            _context = context;
            _signInManager = signInManager;
            _logger = logger;
        }



        public void OnGet(int id)
        {
            Subforum = _context.Subforums.Where(sub => sub.ID == id).FirstOrDefault();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            try { 
            var currentTime = DateTime.Now;
            var subforum = await _context.Subforums.Where(sub => sub.ID == id)
                                                   .Include(sub => sub.Threads)
                                                   .ThenInclude(thread => thread.Replies)
                                                   .FirstOrDefaultAsync();
            var user = await _userManager.GetUserAsync(User);

            Reply.DatePosted = currentTime;

            Reply.Author = user;

            Thread.CreatedBy = user;
            Thread.CreatedOn = currentTime;

                subforum.Threads.Add(Thread);
                _context.SaveChanges();
                Thread.Replies = new()
                {
                    Reply
                };
            _context.SaveChanges();

            int newThreadID = Thread.ID;
            return RedirectToPage("Thread", new { id = newThreadID });
            }
            catch(Exception e)
            {
                _logger.LogError(e.ToString() + "\n\n" + e.InnerException);
                return null;
            }
        }
    }
}
