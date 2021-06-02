using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackisDB.Models;
using SnackisForum.Injects;
namespace SnackisForum.Pages
{
    public class MessagesModel : PageModel
    {
        private readonly SnackisContext _context;
        private readonly UserProfile _profile;
        public MessagesModel(SnackisContext context, UserProfile userProfile)
        {
            _context = context;
            _profile = userProfile;
        }

        public List<IGrouping<object, Message>> Messages { get; set; }

        public void OnGet()
        {
            Messages = _context.Messages.Where(message => message.Reciever == _profile.Username || message.Sender == _profile.Username)
                                              .GroupBy(message => new { message.Reciever, message.Sender})
                                              .ToList();
            

        }
    }
}
