using SnackisDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnackisForum
{
    public static class InitialSetup
    {
        public static void Setup(SnackisContext context)
        {
            context.Database.EnsureCreated();
            if(!context.Forums.Any())
            {
                var forums = context.Forums;
                forums.Add(new Forum { 
                    Name = "Bilar",
                    Subforums = new List<Subforum>(){ 
                        new Subforum(){ 
                            Name = "Chevrolet"
                    } 
                    }
                });
                context.SaveChanges();
            }
        }
    }
}
