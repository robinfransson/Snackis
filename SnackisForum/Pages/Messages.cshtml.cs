using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SnackisDB.Models;
using SnackisForum.Injects;
namespace SnackisForum.Pages
{
    public class MessagesModel : PageModel
    {

        #region Readonlies and constuctor
        private readonly SnackisContext _context;
        private readonly UserProfile _profile;
        private readonly ILogger<MessagesModel> _logger;
        public MessagesModel(SnackisContext context, UserProfile userProfile, ILogger<MessagesModel> logger)
        {
            _context = context;
            _profile = userProfile;
            _logger = logger;
        }

        public List<Chat> Chats { get; set; }
        #endregion

        #region On get
        public IActionResult OnGet()
        {
            if (_profile.IsLoggedIn)
            {

            Chats = _context.Chats.Where(chat => chat.Participant1 == _profile.CurrentUser || chat.Participant2 == _profile.CurrentUser)
                                  .Include(Chat => Chat.Messages)
                                  .Include(chat => chat.Participant1)
                                  .Include(chat => chat.Participant2)
                                  .ToList();
                return Page();
            }
            return RedirectToPage("~/");

        }
        #endregion

 
        #region Send message
        public IActionResult OnPost(string recipient, string title, string message)
        {
            var reciever = _context.Users.FirstOrDefault(user => user.UserName == recipient);
            var currentUser = _context.Users.FirstOrDefault(user => user.UserName == _profile.Username);
            Chat chat = _context.Chats.Where(chat => chat.Participant1 == reciever && chat.Participant2 == currentUser || chat.Participant2 == reciever && chat.Participant1 == currentUser)
                                      .Include(chat => chat.Messages).FirstOrDefault();

            if (chat is null)
            {
                chat = new()
                {
                    Messages = new(),
                    Participant1 = currentUser,
                    Participant2 = reciever
                };

                _context.Chats.Add(chat);
            }

            chat.Messages.Add(new()
            {
                MessageTitle = title,
                MessageBody = message,
                DateSent = DateTime.Now,
                Sender = currentUser.UserName

            });

            _context.SaveChanges();
            return RedirectToPage();

        }

        #endregion

    }
}
