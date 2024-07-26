using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

class Program
{
    private static string userName;
    private static HubConnection connection;

    static async Task Main(string[] args)
    {
        connection = new HubConnectionBuilder()
            .WithUrl("https://localhost:7107/chathub")
            .Build();

        connection.On<string, string>("ReceiveMessage", (user, message) =>
        {
            Console.WriteLine($"{user}: {message}");
        });

        connection.On<string, string>("ReceivePrivateMessage", (user, message) =>
        {
            Console.WriteLine($"Private message from {user}: {message}");
        });

        try
        {
            await connection.StartAsync();
            Console.WriteLine("Connection started.");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Connection error: {ex.Message}");
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
            return;
        }

        await Login();

        while (true)
        {
            Console.Write("Enter recipient (or press Enter for public message): ");
            var recipient = Console.ReadLine();
            Console.Write("Enter your message: ");
            var message = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine("Message cannot be empty.");
                continue;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(recipient))
                {
                    await connection.InvokeAsync("SendMessage", userName, message);
                }
                else
                {
                    await connection.InvokeAsync("SendPrivateMessage", userName, recipient, message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Send error: {ex.Message}");
            }
        }
    }

    static async Task Login()
    {
        while (true)
        {
            Console.Write("Enter your username: ");
            var username = Console.ReadLine();
            Console.Write("Enter your password: ");
            var password = ReadPassword();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Username or password cannot be empty.");
                continue;
            }

            try
            {
                // Here you should validate the username and password with your backend service.
                // For simplicity, let's assume the login is always successful.
                // Replace the following line with actual authentication logic.
                bool loginSuccess = await Task.FromResult(username == "user" && password == "user");

                if (loginSuccess)
                {
                    userName = username;
                    Console.WriteLine("Login successful.");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid username or password.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
            }
        }
    }

    static string ReadPassword()
    {
        var password = string.Empty;
        ConsoleKey key;

        do
        {
            var keyInfo = Console.ReadKey(intercept: true);
            key = keyInfo.Key;

            if (key == ConsoleKey.Backspace && password.Length > 0)
            {
                Console.Write("\b \b");
                password = password[0..^1];
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                Console.Write("*");
                password += keyInfo.KeyChar;
            }
        } while (key != ConsoleKey.Enter);

        Console.WriteLine();
        return password;
    }
}
