using System;

namespace TelegramBot
{
    class Program
    {
        static void Main(string[] args)
        {
            TelegramClient telegramClient = new TelegramClient();
            telegramClient.StartClient();
            Console.ReadLine();
        }
    }
}
