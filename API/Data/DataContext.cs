using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //configure UserLike table manually
            builder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.LikedUserId }); //set primary key

            builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers) // one source user can like many users
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade); // delete a user also delete related entities.

            builder.Entity<UserLike>()
                .HasOne(s => s.LikedUser) //被按讚的人
                .WithMany(l => l.LikedByUsers) //有誰按讚
                .HasForeignKey(s => s.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessageReceived)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessageSent)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}