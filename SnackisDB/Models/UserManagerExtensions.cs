using Microsoft.AspNetCore.Identity;
using SnackisDB.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnackisDB.Models
{
    public static class UserManagerExtensions
    {
        public static SnackisUser FindByUsername(this SnackisContext context,string username)
        {
            return context.Users.FirstOrDefault(user => user.UserName == username);
        }

    }
}
