using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;

namespace _9_MyAcademy_MVC_CodeFirst.Services
{
    public class TavilyService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public TavilyService()
        {
            _apiKey = "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";
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<string> SearchWeb(string query)
        {
            try
            {
                var url = "https://api.tavily.com/search";

                var requestBody = new
                {
                    api_key = _apiKey,
                    query = query,
                    search_depth = "basic",
                    max_results = 3
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Debug.WriteLine("Tavily API Request: " + query);

                var response = await _httpClient.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                Debug.WriteLine("Tavily Response Status: " + response.StatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    return "Error: " + response.StatusCode;
                }

                dynamic result = JsonConvert.DeserializeObject(responseString);

                if (result?.results != null && result.results.Count > 0)
                {
                    var answer = result.answer?.ToString();
                    if (!string.IsNullOrEmpty(answer))
                    {
                        return answer;
                    }

                    var firstResult = result.results[0];
                    var title = firstResult?.title?.ToString() ?? "";
                    var snippet = firstResult?.content?.ToString() ?? "";
                    var url_result = firstResult?.url?.ToString() ?? "";

                    return $"{title}\n\n{snippet}\n\nSource: {url_result}";
                }

                return "No results found.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Tavily Error: " + ex.Message);
                return "Error: " + ex.Message;
            }
        }
    }
}
