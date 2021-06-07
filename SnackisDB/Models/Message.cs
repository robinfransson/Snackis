using SnackisDB.Models.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
