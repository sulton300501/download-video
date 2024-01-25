using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.InlineQueryResults;


namespace TelegramBotExample
{
    public class TelegramBotClass
    {
        public string Token;
        public string channelName;
        public string postText;
        public string photoOrVideoUrl;
        public DateTime postCreationTime;
        public bool isEditingPost;
        public readonly string[] sites = { "Google", "Github", "Telegram", "Wikipedia" };
        public  string[] siteDescriptions =
        {
    "Google is a search engine",
    "Github is a git repository hosting",
    "Telegram is a messenger",
    "Wikipedia is an open wiki"
};


        public TelegramBotClass(string token)
        {
            this.Token = token;
        }

        public async Task BotHandle()
        {
            var botClient = new TelegramBotClient(Token);
            using CancellationTokenSource cts = new();

            botClient.StartReceiving(
                updateHandler:  HandleUpdateAsync,
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

            var chatId = update.Message.Chat.Id;
     



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
            /*else if (update.Message.Text.StartsWith("https"))
            {
                var fileId = update.Message.Photo.Last().FileId;
                var fileInfo = await botClient.GetFileAsync(fileId);
                var filePath = fileInfo.FilePath;

                const string destinationFilePath = "../downloaded.file";

                await using Stream fileStream = System.IO.File.Create(destinationFilePath);
                var file = await botClient.GetInfoAndDownloadFileAsync(
                    fileId: fileId,
                    destination: fileStream,
                    cancellationToken: cancellationToken);

            }
            else*/
                Task BotOnChosenInlineResultReceived(ITelegramBotClient bot, ChosenInlineResult chosenInlineResult)
            {
                if (uint.TryParse(chosenInlineResult.ResultId, out var resultId) // check if a result id is parsable and introduce variable
                    && resultId < sites.Length)
                {
                    Console.WriteLine($"User {chosenInlineResult.From} has selected site: {sites[resultId]}");
                }

                return Task.CompletedTask;
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
