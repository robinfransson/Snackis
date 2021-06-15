using SnackisDB.Models.Identity;
using System;
using System.Collections.Generic;

namespace SnackisDB.Models
{
    public class ForumThread
    {
        public int ID { get; set; }
        public SnackisUser CreatedBy { get; set; }
        public int Views { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public virtual Subforum Parent { get; set; }
        public List<ForumReply> Replies { get; set; }
        public DateTime CreatedOn { get; set; }
        //public ForumReply LastReply { get; set; }/*=> this.Replies.OrderBy(reply => reply.DatePosted).FirstOrDefault();*/


        //public string DaysAgo()
        //{

        //    string hours = this.CreatedOn.Hour.ToString();
        //    string minutes = this.CreatedOn.Minute.ToString();
        //    var now = DateTime.Now;
        //    var elasped = now.Subtract(CreatedOn);
        //    double daysAgo = elasped.TotalDays;

        //    var numbersToAddZeroTo = Enumerable.Range(1, 10);
        //    if (numbersToAddZeroTo.Contains(CreatedOn.Hour))
        //    {
        //        hours = "0" + hours;
        //    }
        //    if (numbersToAddZeroTo.Contains(CreatedOn.Minute))
        //    {
        //        minutes = "0" + minutes;
        //    }

        //    if (daysAgo <= 1 && now.ToShortDateString() == this.CreatedOn.ToShortDateString())
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
        //        return $"{CreatedOn.ToShortTimeString()} {hours}:{minutes}";
        //    }
        //}


    }
}