using SnackisDB.Models.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SnackisDB.Models
{
    public class Forum
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public List<SnackisUser> Moderators { get; set; }
        public List<Subforum> Subforums { get; set; }
    }
}
