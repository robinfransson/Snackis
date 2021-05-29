using SnackisDB.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackisDB.Models
{
    public class ForumReply
    {
        public string DaysAgo()
        {

            string hours = this.DatePosted.Hour.ToString();
            string minutes = this.DatePosted.Minute.ToString();
            var now = DateTime.Now;
            var elasped = now.Subtract(DatePosted);
            double daysAgo = elasped.TotalDays;

            var numbersToAddZeroTo = Enumerable.Range(0, 9);
            if (numbersToAddZeroTo.Contains(DatePosted.Hour))
            {
                hours = "0" + hours;
            }
            if (numbersToAddZeroTo.Contains(DatePosted.Minute))
            {
                minutes = "0" + minutes;
            }

            if (daysAgo <= 1)
            {
                return $"idag {hours}:{minutes}";
            }
            else if (daysAgo <= 2)
            {
                return $"igår {hours}:{minutes}";
            }
            else
            {
                return $"för {Math.Round(daysAgo,0)} dagar sedan";
            }
        }
        public int ID { get; set; }
        public virtual ForumThread ParentThread { get; set; }

        public virtual ForumReply ParentComment { get; set; }

        public virtual List<ForumReply> Replies { get; set; }

        public string Content { get; set; }
        public string ReplyTitle { get; set; }
        public virtual SnackisUser Poster { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
