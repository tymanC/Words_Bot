using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Words_Bot.Constants;


namespace Words_Bot.SynonimAPI.SClient
{
    public class SClient
    {
		private static string _apiKey;
		private static string _apiHost;
		public SClient()
        {
			_apiKey = Constant.apiKey;
			_apiHost = Constant.apiHost;
        }
		public async Task<SModel> GetSynonimOfWord(string word)
		{
			var client = new HttpClient();
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri($"https://localhost:7166/Synonym?word={word}"),
				Headers=
				{
						{ "X-RapidAPI-Key", _apiKey },
						{ "X-RapidAPI-Host", _apiHost },
				},
			};
			var response = await client.SendAsync(request);
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<SModel>(result);
		}
	}
}
