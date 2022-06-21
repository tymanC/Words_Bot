using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Words_Bot.AnagramsApi.AnaModels
{
    public class AnaModel
    {
        public List<Result> Result { get; set; }
    }
    public class Result
    {
        public string Anagram { get; set; }
        public string Numwords { get; set; }
    }
}
