using ChatAppSignalR.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatAppSignalR.Chat
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ChatDBContext(
                serviceProvider.GetRequiredService<DbContextOptions<ChatDBContext>>());
            if (!context.ApplicationUsers.Any())
            {
                context.ApplicationUsers.AddRange(
                    new ApplicationUser
                    {
                        Username = "admin",
                        Password = "admin"
                    },
                    new ApplicationUser
                    {
                        Username = "user",
                        Password = "user"
                    });
                context.SaveChanges();
            }
        }
    }
}
