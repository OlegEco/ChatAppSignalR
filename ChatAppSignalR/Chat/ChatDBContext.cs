using ChatAppSignalR.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace ChatAppSignalR.Chat
{
    public class ChatDBContext : DbContext
    {
        public ChatDBContext(DbContextOptions<ChatDBContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });
        }
    }
}
