using ChatAppSignalR.Chat;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatAppSignalR.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatDBContext _dbContext;

        public ChatHub(ChatDBContext context)
        {
            _dbContext = context;
        }

        public async Task SendMessage(string user, string message, string recipient)
        {
            var chatMessage = new ChatMessage
            {
                Id = Guid.NewGuid(),
                Message = message,
                Recipient = recipient,
                TimeStamp = DateTime.UtcNow,
                User = user
            };

            _dbContext.ChatMessages.Add(chatMessage);
            await _dbContext.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveMessage", user, message, recipient);
        }

        public async Task SendPrivateMessage(string user, string recipient, string message)
        {
            var chatMessage = new ChatMessage
            {
                User = user,
                Message = message,
                TimeStamp = DateTime.UtcNow,
                Recipient = recipient  // Вказуємо отримувача для приватних повідомлень
            };

            _dbContext.ChatMessages.Add(chatMessage);
            await _dbContext.SaveChangesAsync();

            var recipientConnectionId = Context.UserIdentifier;  // Отримати ідентифікатор користувача
            await Clients.User(recipientConnectionId).SendAsync("ReceiveMessage", user, message, recipient);
        }
    }
}
