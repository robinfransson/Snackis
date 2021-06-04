using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SnackisForum.Injects;
using SnackisDB.Models.Identity;
using SnackisDB.Models;

namespace SnackisForum.Pages.Admin
{
    public class IndexModel : PageModel
    {

        public List<Forum> Forums { get; set; }
        public List<Subforum> Subforums { get; set; }
        public int Users { get; set; }
        public int Reports { get; set; }
        public IActionResult OnGet([FromServices] UserProfile userProfile)
        {
            if (!userProfile.IsLoggedIn)
            {
               return Redirect("../Index");
            }
            else if (!userProfile.IsAdmin)
            {
                return Redirect("../Index");
            }


            return Page();
        }
    }
}
