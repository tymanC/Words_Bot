using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Words_Bot.AntonymApi.AModels;
using Words_Bot.Constants;

namespace Words_Bot.AntonymApi.AClients
{
    public class AClient
    {
		private static string _apiKey;
		private static string _apiHost;
		public AClient()
		{
			_apiKey = Constant.apiKey;
			_apiHost = Constant.apiHost;
		}
		public async Task<AModel> GetAntonymOfWord(string word)
		{
			var client = new HttpClient();
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri($"https://localhost:7166/Antonym?word={word}"),
				Headers =
				{
						{ "X-RapidAPI-Key", _apiKey },
						{ "X-RapidAPI-Host", _apiHost },
				},
			};
			var response = await client.SendAsync(request);
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<AModel>(result);
		}
	}
}
