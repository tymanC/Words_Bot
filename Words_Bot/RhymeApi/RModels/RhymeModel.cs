using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Words_Bot.RhymeApi.RModels
{
    public class RhymeModel
    {
        public string Word { get; set; }
        public Rhymes Rhymes { get; set; }
    }
    public class Rhymes
    {
        public List<string> All { get; set; }
    }
}
