using ChatAppSignalR.Chat;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatAppSignalR.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatDBContext _dbContext;

        public ChatHub(ChatDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task SendMessage(string user, string message)
        {
            var chatMessage = new ChatMessage
            {
                User = user,
                Message = message,
                TimeStamp = DateTime.Now,
                Recipient = null
            };
            _dbContext.ChatMessages.Add(chatMessage);
            await _dbContext.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public async Task SendPrivateMessage(string user, string recipient, string message)
        {
            var chatMessage = new ChatMessage
            {
                User = user,
                Message = message,
                TimeStamp = DateTime.Now,
                Recipient = recipient,
            };
            _dbContext.ChatMessages.Add(chatMessage);
            await _dbContext.SaveChangesAsync();

            await Clients.User(recipient).SendAsync("ReceiveMessage", user, message);
        }

        public override Task OnConnectedAsync()
        {
            var userName = Context.User.Identity.Name;
            Clients.All.SendAsync("UserConnected", userName);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userName = Context.User.Identity.Name;
            Clients.All.SendAsync("UserDisconnected", userName);
            return base.OnDisconnectedAsync(exception);
        }
    }
}