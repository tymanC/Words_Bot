using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Words_Bot.TranslateApi.Model
{
    public class TrModel
    {
        public Data data { get; set; }

    }
    public class Translation
    {
        public string translatedText { get; set; }
    }
    public class Data
    {
        public List<Translation> translations { get; set; }

    }
    
}
