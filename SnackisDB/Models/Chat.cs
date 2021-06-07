using SnackisDB.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackisDB.Models
{
    public class Chat
    {
        public int ID { get; set; }
        public SnackisUser Participant1 { get; set; }
        public SnackisUser Participant2 { get; set; }
        public List<Message> Messages { get; set; }


        public bool UserIsAParticipant(SnackisUser user)
        {
            return Participant1 == user || Participant2 == user;
        }
    }
}
