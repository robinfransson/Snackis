using SnackisDB.Models.Identity;
using System;

namespace SnackisDB.Models
{
    public class Report
    {
        public int ID { get; set; }
        public SnackisUser Reporter { get; set; }
        public ForumReply ReportedReply { get; set; }
        public ForumThread ReportedThread { get; set; }
        public bool ActionTaken { get; set; }
        public bool Removed { get; set; }
        public DateTime DateReported { get; set; }
    }
}
