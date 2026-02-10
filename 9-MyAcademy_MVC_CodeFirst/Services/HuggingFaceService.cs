using System;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace _9_MyAcademy_MVC_CodeFirst.Services
{
    public class HuggingFaceService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        private const string API_URL = "https://api-inference.huggingface.co/models/";
        private const string CLASSIFICATION_MODEL = "facebook/bart-large-mnli";

        public HuggingFaceService()
        {
            _apiKey = ConfigurationManager.AppSettings["HuggingFaceApiKey"] ?? "";
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(30);

            if (!string.IsNullOrEmpty(_apiKey))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + _apiKey);
            }
        }

        public async Task<MessageClassificationResult> ClassifyMessage(string message)
        {
            try
            {
                var categories = new[]
                {
                    "complaint",
                    "inquiry",
                    "thank you",
                    "request",
                    "feedback",
                    "urgent"
                };

                var url = API_URL + CLASSIFICATION_MODEL;

                var requestBody = new
                {
                    inputs = message,
                    parameters = new
                    {
                        candidate_labels = categories,
                        multi_label = true
                    }
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Debug.WriteLine("Sending classification request to Hugging Face...");

                var response = await _httpClient.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                Debug.WriteLine("HuggingFace Response Status: " + response.StatusCode);
                Debug.WriteLine("HuggingFace Response: " + responseString);

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("HuggingFace API Error: " + response.StatusCode);
                    return FallbackClassification(message);
                }

                var result = JsonConvert.DeserializeObject<ZeroShotApiResponse>(responseString);

                if (result != null && result.Labels != null && result.Labels.Length > 0)
                {
                    var topLabel = result.Labels[0];
                    var topScore = result.Scores[0];

                    bool isUrgent = false;
                    int urgentIndex = Array.IndexOf(result.Labels, "urgent");
                    if (urgentIndex >= 0 && result.Scores[urgentIndex] > 0.5)
                    {
                        isUrgent = true;
                    }

                    var allCategories = new List<CategoryScoreItem>();
                    for (int i = 0; i < result.Labels.Length; i++)
                    {
                        allCategories.Add(new CategoryScoreItem
                        {
                            CategoryEnglish = result.Labels[i],
                            Category = MapCategory(result.Labels[i]),
                            Score = Math.Round(result.Scores[i] * 100, 1)
                        });
                    }

                    return new MessageClassificationResult
                    {
                        Category = MapCategory(topLabel),
                        CategoryEnglish = topLabel,
                        Confidence = Math.Round(topScore * 100, 1),
                        IsUrgent = isUrgent,
                        AllCategories = allCategories
                    };
                }

                return FallbackClassification(message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in ClassifyMessage: " + ex.Message);
                return FallbackClassification(message);
            }
        }

        private string MapCategory(string englishCategory)
        {
            switch (englishCategory.ToLower())
            {
                case "complaint": return "Complaint";
                case "inquiry": return "Inquiry";
                case "thank you": return "Thank You";
                case "request": return "Request";
                case "feedback": return "Feedback";
                case "urgent": return "Urgent";
                default: return "General";
            }
        }

        private MessageClassificationResult FallbackClassification(string message)
        {
            var lowerMessage = message.ToLower();

            string category = "General";
            string categoryEn = "general";

            if (lowerMessage.Contains("thank") || lowerMessage.Contains("appreciate") || lowerMessage.Contains("grateful"))
            {
                category = "Thank You";
                categoryEn = "thank you";
            }
            else if (lowerMessage.Contains("complaint") || lowerMessage.Contains("problem") || lowerMessage.Contains("issue") || lowerMessage.Contains("bad") || lowerMessage.Contains("terrible") || lowerMessage.Contains("unhappy"))
            {
                category = "Complaint";
                categoryEn = "complaint";
            }
            else if (lowerMessage.Contains("urgent") || lowerMessage.Contains("asap") || lowerMessage.Contains("immediately") || lowerMessage.Contains("emergency"))
            {
                category = "Urgent";
                categoryEn = "urgent";
            }
            else if (lowerMessage.Contains("?") || lowerMessage.Contains("how") || lowerMessage.Contains("what") || lowerMessage.Contains("when") || lowerMessage.Contains("information"))
            {
                category = "Inquiry";
                categoryEn = "inquiry";
            }
            else if (lowerMessage.Contains("please") || lowerMessage.Contains("want") || lowerMessage.Contains("need") || lowerMessage.Contains("would like"))
            {
                category = "Request";
                categoryEn = "request";
            }
            else if (lowerMessage.Contains("suggest") || lowerMessage.Contains("recommend") || lowerMessage.Contains("opinion") || lowerMessage.Contains("think"))
            {
                category = "Feedback";
                categoryEn = "feedback";
            }

            return new MessageClassificationResult
            {
                Category = category,
                CategoryEnglish = categoryEn,
                Confidence = 70.0,
                IsUrgent = category == "Urgent",
                AllCategories = new List<CategoryScoreItem>
                {
                    new CategoryScoreItem { Category = category, CategoryEnglish = categoryEn, Score = 70.0 }
                }
            };
        }
    }

    public class ZeroShotApiResponse
    {
        [JsonProperty("sequence")]
        public string Sequence { get; set; }

        [JsonProperty("labels")]
        public string[] Labels { get; set; }

        [JsonProperty("scores")]
        public double[] Scores { get; set; }
    }

    public class MessageClassificationResult
    {
        public string Category { get; set; }
        public string CategoryEnglish { get; set; }
        public double Confidence { get; set; }
        public bool IsUrgent { get; set; }
        public List<CategoryScoreItem> AllCategories { get; set; }
    }

    public class CategoryScoreItem
    {
        public string Category { get; set; }
        public string CategoryEnglish { get; set; }
        public double Score { get; set; }
    }
}
