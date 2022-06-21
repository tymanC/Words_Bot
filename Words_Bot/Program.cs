using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Words_Bot.Bot;
using Words_Bot.TrApi.TrClients;
using Words_Bot.RhymeApi.RClients;
namespace Words_Bot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //RClient rClient = new RClient();    
            //TrClient trClient = new TrClient();
            //string s = "любов";
            //string eng = trClient.CheapTr("uk", s, "en").Result.translatedText;
            //string rhyme = rClient.GetRhymeOfWord(eng).Result.Rhymes.All[0];
            //Console.WriteLine(trClient.CheapTr("en", rhyme, "uk").Result.translatedText.ToString());

            TgBot tgBot = new TgBot();
            
            tgBot.Start();

            Console.ReadKey();
        }
    }
}
