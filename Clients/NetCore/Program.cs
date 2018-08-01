using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace NetCore
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Press Ctrl+C to terminate.");

            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5001/hubs/values")
                .Build();
            connection.On<string>("receiveMessage", message =>
            {
                Console.WriteLine($"Received: {message}");
            });
            await connection.StartAsync();
            var connectionId = await connection.InvokeAsync<string>("GetConnectionId");

            while (true)
            {
                Console.WriteLine("Message:");
                var message = Console.ReadLine();
                await Task.Run(async () =>
                {
                    using(var client = new HttpClient())
                    {
                        await client.GetAsync($"http://localhost:5001/api/values/{connectionId}/{message}");
                    }
                });
            }
        }
    }
}
