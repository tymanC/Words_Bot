using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using Words_Bot.RhymeApi.RModels;
using Words_Bot.Constants;

namespace Words_Bot.RhymeApi.RClients
{
    public class RClient
    {
		private static string _apiKey;
		private static string _apiHost;
		public RClient()
        {
			_apiKey = Constant.apiKey;
			_apiHost = Constant.apiHost;
		}
		public async Task<RhymeModel> GetRhymeOfWord(string word)
		{
			var client = new HttpClient();
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri($"https://localhost:7166/Rhyme?word={word}"),
				Headers =
				{
					{ "X-RapidAPI-Host", _apiHost},
					{ "X-RapidAPI-Key", _apiKey },
				},
			};
			var response = await client.SendAsync(request);
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<RhymeModel>(result);
		}
	}
}
