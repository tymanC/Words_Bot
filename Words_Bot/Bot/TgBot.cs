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
using System.Text.RegularExpressions;
using Words_Bot.RhymeApi.RClients;
using Words_Bot.SynonimAPI.SClient;
using Words_Bot.AntonymApi.AClients;
using Words_Bot.AnagramsApi.AnaClients;
using VoiceRSS_SDK;
using System.IO;
using Words_Bot.TrApi.TrClients;


namespace Words_Bot.Bot
{
    public class TgBot
    {
        RClient RClient = new RClient();
        TrClient TrClient = new TrClient();
        SClient sClient = new SClient();
        AClient aClient = new AClient();
        AnaClient anaClient = new AnaClient(); 
        TelegramBotClient botClient = new TelegramBotClient("5506281902:AAE9n3RYpTBa4ep1IJPjeTLxphoB172dFcE");
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
            bool InputError = false;
            if (message.Text == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Введіть вірш");
                return;
            }
            else
            if (message.Text !=null)
            {
                string poetry = message.Text;
                var text = TrClient.CheapTr("uk", poetry, "en").Result.translatedText;
                
                Regex r = new Regex("\\([a-zA-Z]+, [a-zA-Z]+\\)", RegexOptions.IgnoreCase);
                try
                {
                    MatchCollection matches = r.Matches(text);
                }
                catch
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, $"Помилка введення даних.\n Спробуйте відправити вірш із ключовими словами у вигляді (Синонім, слово).\n" +
                        $"Обов'язково з дужками і комою.\n Варіанти для першого слова: Синонім/Антонім/Анаграма/Рима;\nзамість другого слова необхідно вказати слово, до якого " +
                        $"необхідно знайти заміну по попередньо вказаному слові\n\nВведіть /start, щоб спробувати ще раз.");
                    InputError = true;
                }
                //if(matches.Count == 0)
                //{
                //    await botClient.SendTextMessageAsync(message.Chat.Id, $"Помилка введення даних.\n Спробуйте відправити вірш із ключовими словами у вигляді (Синонім, слово).\n" +
                //        $"Обов'язково з дужками і комою.\n Варіанти для першого слова: Синонім/Антонім/Анаграма/Рима;\nзамість другого слова необхідно вказати слово, до якого " +
                //        $"необхідно знайти заміну по попередньо вказаному слові\n\nВведіть /start, щоб спробувати ще раз.");
                //   // InputError=true;
                //}
                if(InputError!=true)
                {
                    try
                    {
                        //var engRes = GetKey(text);
                        //Console.WriteLine($"Translated {text}");
                        var translatedresult = TrClient.CheapTr("en", GetKey(text), "uk").Result.translatedText;

                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Your poetry:\n{translatedresult}");
                    }
                    catch (AggregateException err)
                    {

                        foreach (var errInner in err.InnerExceptions)
                        {
                            Console.WriteLine(errInner); //this will call ToString() on the inner execption and get you message, stacktrace and you could perhaps drill down further into the inner exception of it if necessary 
                        }
                        await botClient.SendTextMessageAsync(message.Chat.Id, $"Помилка перекладача. Спробуйте ще раз через декілька хвилин.");
                    }
                }
                
            }
            //else if (message.Text != null)
            //{
            //    string poetry = message.Text;
            //    try
            //    {
            //        await botClient.SendTextMessageAsync(message.Chat.Id, $"Your poetry:\n{GetKey(poetry)}");
            //    }
            //    catch
            //    {
            //        await botClient.SendTextMessageAsync(message.Chat.Id, $"Error");
            //    }
            //}


        }
        
        string GetKey(string poetry)
        {
            string error = "";
            


            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            Regex r = new Regex("\\([a-zA-Z]+, [a-zA-Z]+\\)", RegexOptions.IgnoreCase);

            MatchCollection matches = r.Matches(poetry);

            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>();
                    string[] splitKeyWord = match.ToString().Split(',');
                    //foreach (string word in splitKeyWord) Console.Write(word);
                    string s = splitKeyWord[1].Remove(0, 1);
                    dict.Add(splitKeyWord[0].Remove(0, 1), s.Remove(s.Length - 1));
                    foreach (KeyValuePair<string, string> para in dict)
                    {
                        Console.WriteLine(para.Key + para.Value);
                    }
                    list.Add(dict);
                }

            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
            //bool rhymeKey = false;
            foreach (var dict in list)
            {
                foreach (var para in dict)
                {
                    if (para.Key != null && para.Value != null)
                    {
                        if (para.Key == "Rome")
                        {
                            Regex rhyme = new Regex("\\(Rome, [a-zA-Z]+\\)", RegexOptions.IgnoreCase);
                            MatchCollection rmc = rhyme.Matches(poetry);
                            if (rmc.Count > 0)
                            {
                                try
                                {
                                    poetry = poetry.Replace(rmc[0].ToString(), RClient.GetRhymeOfWord(para.Value).Result.Rhymes.All[0]);
                                }
                                catch
                                {
                                    error = "Неможливо знайти риму";
                                }

                            }

                        }
                        else if (para.Key == "Synonym")
                        {
                            Regex rhyme = new Regex("\\(Synonym, [a-zA-Z]+\\)", RegexOptions.IgnoreCase);
                            MatchCollection rmc = rhyme.Matches(poetry);
                            if (rmc.Count > 0)
                            {
                                try
                                {
                                    poetry = poetry.Replace(rmc[0].ToString(), sClient.GetSynonimOfWord(para.Value).Result.synonyms[0]);
                                }
                                catch
                                {
                                    error = "Неможливо знайти синонім";
                                }
                            }


                        }
                        else if (para.Key == "Antonym")
                        {
                            Regex rhyme = new Regex("\\(Antonym, [a-zA-Z]+\\)", RegexOptions.IgnoreCase);
                            MatchCollection rmc = rhyme.Matches(poetry);
                            if (rmc.Count > 0)
                            {
                                try
                                {
                                    poetry = poetry.Replace(rmc[0].ToString(), aClient.GetAntonymOfWord(para.Value).Result.Antonyms[0]);
                                }
                                catch
                                {
                                    error = "Неможливо знайти антонім";
                                }

                            }
                        }
                        else if (para.Key == "Anagram")
                        {
                            Regex rhyme = new Regex("\\(Anagram, [a-zA-Z]+\\)", RegexOptions.IgnoreCase);
                            MatchCollection rmc = rhyme.Matches(poetry);
                            if (rmc.Count > 0)
                            {
                                try
                                {
                                    poetry = poetry.Replace(rmc[0].ToString(), anaClient.GetAnagramOfWord(para.Value).Result.Result[0].Anagram);
                                }
                                catch
                                {
                                    error = "Неможливо знайти анаграму";
                                }

                            }
                        }
                        else
                        {
                            error = "Помилка введення даних. Перевірте правопис слів у дужках";
                        }
                    }

                }
            }
            
            if (error != "")
                return error;
            else return poetry;
            
        }

    }
}
