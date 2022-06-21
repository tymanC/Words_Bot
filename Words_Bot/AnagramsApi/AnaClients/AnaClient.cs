using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Words_Bot.AnagramsApi.AnaModels;
using Words_Bot.Constants;

namespace Words_Bot.AnagramsApi.AnaClients
{
    public class AnaClient
    {
        private static string _token;
        private static string _userID;
        private HttpClient _client;
        public AnaClient()
        {
            _token = Constant.token;
            _userID = Constant.userId;
            _client = new HttpClient();
        }
        public async Task<AnaModel> GetAnagramOfWord(string word)
        {
            var response = await _client.GetAsync($"https://localhost:7166/Anagram?word={word}");
            var content = response.Content.ReadAsStringAsync().Result;
            var result = JsonConvert.DeserializeObject<AnaModel>(content);//десеріалізували в тип даних weathercity
            return result;
        }

    }
}
