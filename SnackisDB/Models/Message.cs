using System;
using System.ComponentModel;

namespace SnackisDB.Models
{
    public class Message
    {
        public int ID { get; set; }
        public Chat Chat { get; set; }
        public string Sender { get; set; }
        public string MessageTitle { get; set; }
        public string MessageBody { get; set; }
        [DefaultValue(false)]
        public bool HasBeenViewed { get; set; }
        public DateTime DateSent { get; set; }
    }
}
