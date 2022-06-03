using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Exceptions;
using Words_Bot.TranslateApi.Clients;

namespace Words_Bot.Bot
{
    public class TgBot
    {
        Tr_Client Tr_Client = new Tr_Client();
        TelegramBotClient botClient = new TelegramBotClient("5325704722:AAEhgqsJQqt2mrCYvhPUsAm_0O53MnlM4tA");
        CancellationToken cancellationToken = new CancellationToken();
        ReceiverOptions receiverOptions = new ReceiverOptions { AllowedUpdates = { } };

        //start bot
        public async Task Start()
        {
            botClient.StartReceiving(HandlerUpdateAsync, HandlerError, receiverOptions, cancellationToken);
            var botMe = await botClient.GetMeAsync();
            Console.WriteLine($"Бот {botMe.Username} почав працювати");
            Console.ReadKey();
        }

        private Task HandlerError(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Помилка в телеграм бот АПІ:\n{apiRequestException.ErrorCode}" +
                $"{apiRequestException.Message}", _ => exception.ToString()
            };
            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private async Task HandlerUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message && update?.Message?.Text != null)
            {
                //запуск методу, що обробляє текст
                await HandlerMessageAsync(botClient, update.Message);
            }

        }

        private async Task HandlerMessageAsync(ITelegramBotClient botClient, Message message)
        {
            if (message.Text == "/start")
            {
                ReplyKeyboardMarkup replyKeyboardMarkup = new
                    (
                    new[]
                        {
                        new KeyboardButton[] { "Українська мова", "English language" }
                    }
                    )
                {
                    ResizeKeyboard = true
                };
                await botClient.SendTextMessageAsync(message.Chat.Id, "Виберіть пункт меню:", replyMarkup: replyKeyboardMarkup);
                return;
            }else
            if (message.Text == "Українська мова")
            {
               await botClient.SendTextMessageAsync(message.Chat.Id, "Введіть слово для перекладу");
               
            }
            else
            if(message.Text != null)
            {
                try
                {
                    string word = message.Text;
                    var list = Tr_Client.GetTranslationOfWord(word).Result.data.translations;

                    foreach (var item in list)
                    {
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Translated \n{word} - {item.translatedText}");
                    }
                }
                catch
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Виникла помилка. Спробуйте ще раз");
                }
                
                
            }
            if (message.Text == "English language")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "I love you");
            }
           

        }
        void Print()
        {
            string s = " ";
            var list = Tr_Client.GetTranslationOfWord("ло").Result.data.translations;

            foreach (var item in list)
            {
                Console.WriteLine(item.translatedText);
            }

        }
    }
}
