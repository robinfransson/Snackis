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

        public List<IGrouping<int, Message>> Messages { get; set; }

        public void OnGet()
        {
            Messages = _context.Messages.Where(message => message.Reciever == _profile.Username || message.Sender == _profile.Username)
                                              .AsEnumerable()
                                              .GroupBy(message => message.ChatID)
                                              .ToList();
            

        }




        public PartialViewResult OnGetLoadMessages(int id)
        {
            List<Message> chatModel = _context.Messages.Where(message => ((message.Reciever == _profile.Username || message.Sender == _profile.Username) && message.ChatID == id))
                                              .ToList();
            if(!chatModel.Any())
            {
                return null;
            }
            chatModel.Where(message => !message.HasBeenViewed).ToList().ForEach(message => message.HasBeenViewed = true);
            _context.SaveChanges();
            return Partial("_Chat", chatModel);
        }


        public JsonResult OnGetUserExists(string username)
        {
            username = username.ToLower();
            bool exists = _context.Users.Any(user => user.UserName.ToLower() == username);
            return new JsonResult(new { exists });
        }
    }
}
