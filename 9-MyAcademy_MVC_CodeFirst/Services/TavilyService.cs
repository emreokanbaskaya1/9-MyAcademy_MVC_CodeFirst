using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace _9_MyAcademy_MVC_CodeFirst.Services
{
    public class TavilyService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public TavilyService()
        {
            _apiKey = ConfigurationManager.AppSettings["TavilyApiKey"] ?? "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";
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<TavilySearchResult> SearchWeb(string query)
        {
            try
            {
                var url = "https://api.tavily.com/search";

                var requestBody = new
                {
                    api_key = _apiKey,
                    query = query,
                    search_depth = "advanced",
                    include_answer = true,
                    max_results = 5
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Debug.WriteLine("Tavily API Request: " + query);

                var response = await _httpClient.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                Debug.WriteLine("Tavily Response Status: " + response.StatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Tavily Error Response: " + responseString);
                    return new TavilySearchResult { Success = false, ErrorMessage = "API request failed" };
                }

                var result = JsonConvert.DeserializeObject<TavilyApiResponse>(responseString);

                if (result != null)
                {
                    return new TavilySearchResult
                    {
                        Success = true,
                        Answer = result.Answer,
                        Results = result.Results?.Select(r => new SearchResultItem
                        {
                            Title = r.Title,
                            Url = r.Url,
                            Content = r.Content,
                            Score = r.Score
                        }).ToList() ?? new List<SearchResultItem>()
                    };
                }

                return new TavilySearchResult { Success = false, ErrorMessage = "Invalid response" };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Tavily Error: " + ex.Message);
                return new TavilySearchResult { Success = false, ErrorMessage = ex.Message };
            }
        }

        public string FormatInsuranceSearchResults(TavilySearchResult searchResult)
        {
            if (!searchResult.Success || searchResult.Results == null || !searchResult.Results.Any())
            {
                return "I couldn't find current information on this topic.";
            }

            var formatted = new StringBuilder();

            if (!string.IsNullOrEmpty(searchResult.Answer))
            {
                formatted.AppendLine(searchResult.Answer);
                formatted.AppendLine();
            }

            formatted.AppendLine("Here's what I found from reliable sources:");
            formatted.AppendLine();

            int count = 0;
            foreach (var result in searchResult.Results.Take(3))
            {
                count++;
                formatted.AppendLine($"{count}. {result.Title}");
                formatted.AppendLine($"   {result.Content}");
                formatted.AppendLine($"   Source: {result.Url}");
                formatted.AppendLine();
            }

            return formatted.ToString();
        }
    }

    public class TavilyApiResponse
    {
        [JsonProperty("answer")]
        public string Answer { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("results")]
        public List<TavilyResult> Results { get; set; }
    }

    public class TavilyResult
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }
    }

    public class TavilySearchResult
    {
        public bool Success { get; set; }
        public string Answer { get; set; }
        public List<SearchResultItem> Results { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class SearchResultItem
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Content { get; set; }
        public double Score { get; set; }
    }
}
