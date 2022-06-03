using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using Words_Bot.TranslateApi.Model;

namespace Words_Bot.TranslateApi.Clients
{
    public class Tr_Client
    {
		HttpClient client = new HttpClient();
		public async Task<TrModel> GetTranslationOfWord(string word)
		{
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://google-translate1.p.rapidapi.com/language/translate/v2"),
				Headers =
				{
					{ "X-RapidAPI-Host", "google-translate1.p.rapidapi.com" },
					{ "X-RapidAPI-Key", "6ab329991cmsh7bae2aa09e2bf82p1c0d9ajsn97fbf95345b3" },
				},
				Content = new FormUrlEncodedContent(new Dictionary<string, string>
				{
					{ "q", $"{word}" },
					{ "target", "en" },
					{ "source", "uk" },
				}),
			};

			var response = await client.SendAsync(request);
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadAsStringAsync();


			return JsonConvert.DeserializeObject<TrModel>(result);
		}
		
	}
}
