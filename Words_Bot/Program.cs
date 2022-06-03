using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Words_Bot.Bot;

namespace Words_Bot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TgBot tgBot = new TgBot();
            tgBot.Start();
            Console.ReadKey();
        }
    }
}
