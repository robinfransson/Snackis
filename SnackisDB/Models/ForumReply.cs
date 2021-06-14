using SnackisDB.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackisDB.Models
{
    public class ForumReply
    {
        
        public int ID { get; set; }

        public ForumThread Thread { get; set; }
#nullable enable
        public SnackisUser? Author { get; set; }
#nullable disable
        public SnackisUser ReplyTo { get; set; }
        public ForumReply RepliedComment { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ReplyText { get; set; }
        public DateTime DatePosted { get; set; }

        //public string DaysAgo()
        //{

        //    string hours = this.DatePosted.Hour.ToString();
        //    string minutes = this.DatePosted.Minute.ToString();
        //    var now = DateTime.Now;
        //    var elasped = now.Subtract(DatePosted);
        //    double daysAgo = elasped.TotalDays;

        //    var numbersToAddZeroTo = Enumerable.Range(1, 10);
        //    if (numbersToAddZeroTo.Contains(DatePosted.Hour))
        //    {
        //        hours = "0" + hours;
        //    }
        //    if (numbersToAddZeroTo.Contains(DatePosted.Minute))
        //    {
        //        minutes = "0" + minutes;
        //    }

        //    if (daysAgo <= 1 && now.ToShortDateString() == this.DatePosted.ToShortDateString())
        //    {
        //        return $"idag {hours}:{minutes}";
        //    }
        //    else if (daysAgo <= 1)
        //    {
        //        return $"igår {hours}:{minutes}";
        //    }
        //    else if (daysAgo < 8)
        //    {
        //        return $"för {Math.Round(daysAgo, 0)} dagar sedan kl {hours}:{minutes}";
        //    }
        //    else
        //    {
        //        return $"{DatePosted.ToShortTimeString()} {hours}:{minutes}";
        //    }
        //}
    }
}
