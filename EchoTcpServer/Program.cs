using System;
using System.Net;
using System.Threading.Tasks;

namespace EchoTcpServer
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using var server = new EchoServer(IPAddress.Any, 5000);
            _ = Task.Run(() => server.StartAsync());

            using var standardUdpClient = new StandardUdpClient();
            using var sender = new UdpTimedSender("127.0.0.1", 60000, standardUdpClient);

            Console.WriteLine("Press any key to stop sending...");
            sender.StartSending(5000);

            Console.WriteLine("Press 'q' to quit...");
            while (Console.ReadKey(intercept: true).Key != ConsoleKey.Q) { }

            sender.StopSending();
            server.Stop();
        }
    }
}