using Microsoft.AspNetCore.SignalR.Client;

var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7107/chathub")
    .Build();

connection.On<string, string>("ReceiveMessage", (user, message) =>
{
    Console.WriteLine($"{user}: {message}");
});

try
{
    await connection.StartAsync();
    Console.WriteLine("Connection started.");
}
catch (Exception ex)
{
    Console.WriteLine($"Connection error: {ex.Message}");
    return;
}

while (true)
{
    Console.Write("Enter your name: ");
    var user = Console.ReadLine();
    Console.Write("Enter your message: ");
    var message = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(message))
    {
        Console.WriteLine("User or message cannot be empty.");
        continue;
    }

    try
    {
        await connection.InvokeAsync("SendMessage", user, message);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Send error: {ex.Message}");
    }
}
