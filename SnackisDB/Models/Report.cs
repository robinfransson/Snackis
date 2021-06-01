using SnackisDB.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackisDB.Models
{
    class Report
    {
        public int ID { get; set; }
        public SnackisUser Reporter { get; set; }
        public SnackisUser Handler { get; set; }
        public ForumReply ReportedReply { get; set; }
        public ForumThread ReportedThread { get; set; }
        public bool ActionTaken { get; set; }
    }
}
