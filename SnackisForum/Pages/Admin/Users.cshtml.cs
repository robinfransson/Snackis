using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackisDB.Models;
using SnackisDB.Models.Identity;

namespace SnackisForum.Pages.Admin
{
    public class UsersModel : PageModel
    {

        List<SnackisUser> SnackisUsers { get; set; }
        public void OnGet([FromServices] SnackisContext context)
        {
            SnackisUsers = context.Users.ToList();
        }
    }
}
