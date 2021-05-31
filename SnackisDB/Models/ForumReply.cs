﻿using SnackisDB.Models.Identity;
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
        public SnackisUser Author { get; set; }
        public SnackisUser ReplyTo { get; set; }
        public ForumReply RepliedComment { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ReplyText { get; set; }
        public DateTime DatePosted { get; set; }

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
            else if (daysAgo < 8)
            {
                return $"för {Math.Round(daysAgo, 0)} dagar sedan";
            }
            else
            {
                return $"{DatePosted.ToShortTimeString()} {hours}:{minutes}";
            }
        }
    }
}
