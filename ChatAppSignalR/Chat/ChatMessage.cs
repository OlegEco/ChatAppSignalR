namespace ChatAppSignalR.Chat
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string User { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Recipient { get; set; } 
    }
}
