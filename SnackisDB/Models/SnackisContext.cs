

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SnackisDB.Models.Identity;

namespace SnackisDB.Models
{
    public class SnackisContext : IdentityDbContext<SnackisUser>
    {
        public SnackisContext(DbContextOptions<SnackisContext> options) : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Subforum> Subforums { get; set; }
        public DbSet<ForumThread> Threads { get; set; }
        public DbSet<ForumReply> Replies { get; set; }
        public DbSet<Report> Reports { get; set; }

    }
}