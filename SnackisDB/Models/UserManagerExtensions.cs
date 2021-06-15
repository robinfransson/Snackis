using SnackisDB.Models.Identity;
using System.Linq;

namespace SnackisDB.Models
{
    public static class UserManagerExtensions
    {
        public static SnackisUser FindByUsername(this SnackisContext context, string username)
        {
            return context.Users.FirstOrDefault(user => user.UserName == username);
        }

    }
}
