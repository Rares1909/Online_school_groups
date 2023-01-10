using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Online_school.Models;

namespace Online_school.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Group_User> GroupsUsers { get; set; }
        public object Groups_Users { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
            {
                base.OnModelCreating(modelBuilder);
                // definire primary key compus
                modelBuilder.Entity<Group_User>()
                .HasKey(ab => new {
                    ab.UserId,
                    ab.GroupId,
                    
                });
                // definire relatii cu modelele Bookmark si Article (FK)
                modelBuilder.Entity<Group_User>()
                .HasOne(ab => ab.User)
                .WithMany(ab => ab.Groups)
                .HasForeignKey(ab => ab.UserId);
                modelBuilder.Entity<Group_User>()
                .HasOne(ab => ab.Group)
                .WithMany(ab => ab.Users)
                .HasForeignKey(ab => ab.GroupId);
            }
    }

    }