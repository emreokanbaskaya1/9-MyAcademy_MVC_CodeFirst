using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;

namespace _9_MyAcademy_MVC_CodeFirst.Services
{
    public class GeminiService
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public GeminiService()
        {
            _apiKey = ConfigurationManager.AppSettings["GeminiApiKey"] ?? "AIzaSyDWvrtC7YhJgE7q4HKrl6RVTBONuukETok";
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(60);
        }

        public async Task<string> GetInsuranceAdvice(string age, string job, string income, string family)
        {
            try
            {
                var prompt = "You are an insurance advisor. Based on the following information, recommend the most suitable insurance products:\n\n" +
                            "Age: " + age + "\n" +
                            "Occupation: " + job + "\n" +
                            "Monthly Income: " + income + " TL\n" +
                            "Family Status: " + family + "\n\n" +
                            "Please provide 3-4 short and clear insurance recommendations. Answer in English.";

                return await GenerateContent(prompt);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in GetInsuranceAdvice: " + ex.Message);
                return "Error: " + ex.Message;
            }
        }

        private async Task<string> GenerateContent(string prompt)
        {
            try
            {
                // FIXED: Using correct model name from the API list
                var url = "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-001:generateContent?key=" + _apiKey;

                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    },
                    generationConfig = new
                    {
                        temperature = 0.7,
                        topK = 40,
                        topP = 0.95,
                        maxOutputTokens = 1024
                    }
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Debug.WriteLine("Sending request to Gemini API...");
                Debug.WriteLine("URL: " + url);
                
                var response = await _httpClient.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                Debug.WriteLine("Response Status: " + response.StatusCode.ToString());
                Debug.WriteLine("Response Body: " + responseString);

                if (!response.IsSuccessStatusCode)
                {
                    return "API Hatasi: " + response.StatusCode.ToString() + " - " + responseString;
                }

                dynamic result = JsonConvert.DeserializeObject(responseString);
                
                if (result != null && result.candidates != null && result.candidates.Count > 0)
                {
                    var text = result.candidates[0]?.content?.parts[0]?.text;
                    return text != null ? text.ToString() : "Cevap alinamadi.";
                }
                
                return "API'den gecerli bir cevap alinamadi.";
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine("HTTP Error: " + ex.Message);
                return "Internet baglantisi hatasi. Lutfen baglantinizi kontrol edin.";
            }
            catch (TaskCanceledException ex)
            {
                Debug.WriteLine("Timeout Error: " + ex.Message);
                return "Istek zaman asimina ugradi. Lutfen tekrar deneyin.";
            }
            catch (JsonException ex)
            {
                Debug.WriteLine("JSON Error: " + ex.Message);
                return "Veri isleme hatasi. Lutfen tekrar deneyin.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine("General Error: " + ex.Message);
                Debug.WriteLine("Stack Trace: " + ex.StackTrace);
                return "Beklenmeyen hata: " + ex.Message;
            }
        }
    }
}
