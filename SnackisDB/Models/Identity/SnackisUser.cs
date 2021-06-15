using Microsoft.AspNetCore.Identity;
using System;

namespace SnackisDB.Models.Identity
{
    public class SnackisUser : IdentityUser
    {
        public DateTime CreatedOn { get; set; }

        public string ProfileImagePath { get; set; }
    }
}
