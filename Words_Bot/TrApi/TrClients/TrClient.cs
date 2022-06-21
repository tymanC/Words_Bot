using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Words_Bot.TrApi.TrModels;
namespace Words_Bot.TrApi.TrClients
{
    public class TrClient
    {
		public async Task<TrModel> CheapTr(string fromlang, string text, string to)
		{
			var client = new HttpClient();
			TrRequest trRequest = new TrRequest()
			{
				fromLang = fromlang,
				text = text,
				to = to,
			};
			var json = JsonConvert.SerializeObject(trRequest);
			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri($"https://localhost:7166/CheapTr?fromlang={fromlang}&text={text}&to={to}"),
				Headers =
				{
					{ "X-RapidAPI-Key", "6ab329991cmsh7bae2aa09e2bf82p1c0d9ajsn97fbf95345b3" },
					{ "X-RapidAPI-Host", "cheap-translate.p.rapidapi.com" },
				},
				Content = new StringContent(json)
				{
					Headers =
					{
						ContentType = new MediaTypeHeaderValue("application/json")
					}
				}
			};
			var response = await client.SendAsync(request);
			response.EnsureSuccessStatusCode();
			var result = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<TrModel>(result);
		}
	}
}
