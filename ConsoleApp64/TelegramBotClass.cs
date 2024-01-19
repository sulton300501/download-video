using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBotExample
{
    public class TelegramBotClass
    {
        private string Token;
        private string channelName;
        private string postText;
        private string photoOrVideoUrl;
        private DateTime postCreationTime;
        private bool isEditingPost;

        public TelegramBotClass(string token)
        {
            this.Token = token;
        }

        public async Task BotHandle()
        {
            var botClient = new TelegramBotClient(Token);
            using CancellationTokenSource cts = new();

            botClient.StartReceiving(
                updateHandler: async (bot, update, ct) => await HandleUpdateAsync(bot, update, ct),
                pollingErrorHandler: HandlePollingErrorAsync,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            cts.Cancel();
        }

    

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message || message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;


            string replacemessage = update.Message.Text.Replace("www.", "dd");

            if (update.Message.Text.StartsWith("https://www.instagram.com"))
            {
                Message sentMessage = await botClient.SendPhotoAsync(
                    chatId: chatId,
                    photo: replacemessage,
                  //  supportsStreaming: true,
                    cancellationToken: cancellationToken
                );
            }

   











        }

        private Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}
