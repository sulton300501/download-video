
using System;
using TelegramBotExample;

namespace ConsoleApp58;

class Program
{
    static async Task Main(string[] args)
    {
        const string token = "6719531096:AAGHUFOcuu5g-JDh0FT1ecif6yLAYswyAXM";

        TelegramBotClass telegramPost = new TelegramBotClass(token);
        await telegramPost.BotHandle();
    }
}