using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SnackisDB.Models.Identity
{
    public class SnackisUser :  IdentityUser
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ForumUserID { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOn { get; set; }
    }
}
