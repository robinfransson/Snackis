using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SnackisDB.Models.Identity
{
    public class SnackisUser :  IdentityUser
    {
        public DateTime CreatedOn { get; set; }

        public string ProfileImagePath { get; set; }
    }
}
