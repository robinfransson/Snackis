using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnackisDB.Models
{
    public class Subforum
    {

        //public Subforum()
        //{
        //    Threads = new();
        //    LastReply = GetLastReply();
        //}

        //private ForumReply GetLastReply()
        //{
        //    var reply = this.Threads.Select(thread => thread.Replies.OrderByDescending(reply => reply.DatePosted).FirstOrDefault()).FirstOrDefault();
        //    return reply;
        //}
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }

        public List<ForumThread> Threads { get; set; }
        //public ForumReply LastReply { get; set; }
        //public ForumReply LastReply => this.Threads.OrderByDescending(thread => thread.LastReply.DatePosted).Select(thread => thread.LastReply).FirstOrDefault();
    }
}
