using SnackisDB.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}