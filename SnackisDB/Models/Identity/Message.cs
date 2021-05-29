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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [ForeignKey("AspNetUsers")]
        public string RecieverID { get; set; }
        public string SenderID { get; set; }
        public string MessageBody { get; set; }
        [DefaultValue(false)]
        public bool HasBeenViewed { get; set; }

        [DefaultValue(false)]
        public bool MessageReciever { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime Date { get; set; }
    }
}
