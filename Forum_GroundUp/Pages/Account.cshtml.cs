using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using SnackisDB.Models;
using SnackisDB.Models.Identity;

namespace SnackisForum.Pages
{
    public class AccountModel : PageModel
    {
        private readonly SignInManager<SnackisUser> _signInManager;
        private readonly UserManager<SnackisUser> _userManager;
        private readonly ILogger<AccountModel> _logger;
        private readonly SnackisContext _context;

        public AccountModel(SignInManager<SnackisUser> signInManager, UserManager<SnackisUser> userManager, 
                            ILogger<AccountModel> logger, SnackisContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _context = context;
        }
        public void OnGet()
        {
        }




        //public async Task<List<Message>> OnPostGetMessagesAsync()
        //{
        //    string userID = _userManager.GetUserId(User);
        //    return _context.Messages.Where(message => message.UserID == userID).ToList();
        //}



    }
}
