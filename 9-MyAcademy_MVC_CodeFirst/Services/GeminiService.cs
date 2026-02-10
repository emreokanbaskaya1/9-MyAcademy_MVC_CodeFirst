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
        private readonly TavilyService _tavilyService;

        public GeminiService()
        {
            _apiKey = ConfigurationManager.AppSettings["GeminiApiKey"] ?? "AIzaSyDWvrtC7YhJgE7q4HKrl6RVTBONuukETok";
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(60);
            _tavilyService = new TavilyService();
        }

        public async Task<string> GetInsuranceAdvice(string age, string job, string income, string family)
        {
            try
            {
                // Check if user is asking for current prices/data
                var needsRealTimeData = await CheckIfNeedsRealTimeSearch(age, job, income, family);
                
                string contextInfo = "";
                if (needsRealTimeData)
                {
                    Debug.WriteLine("Fetching real-time insurance data from Tavily...");
                    var searchQuery = $"Turkey insurance policy prices {DateTime.Now.Year} {job} profession monthly income {income} TL";
                    var searchResult = await _tavilyService.SearchWeb(searchQuery);
                    
                    if (searchResult.Success)
                    {
                        contextInfo = "\n\nCurrent Market Data (from web search):\n" + 
                                     _tavilyService.FormatInsuranceSearchResults(searchResult);
                    }
                }

                var prompt = "You are an insurance advisor. Based on the following information, recommend the most suitable insurance products:\n\n" +
                            "Age: " + age + "\n" +
                            "Occupation: " + job + "\n" +
                            "Monthly Income: " + income + " TL\n" +
                            "Family Status: " + family + "\n" +
                            contextInfo + "\n\n" +
                            "Please provide 3-4 insurance recommendations with SPECIFIC PRICES if available in the market data above. " +
                            "If you found real prices from web search, use them. Otherwise, provide realistic price estimates based on Turkish market. " +
                            "Format: Product name, coverage details, monthly premium price. Answer in English.";

                return await GenerateContent(prompt);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in GetInsuranceAdvice: " + ex.Message);
                return "Error: " + ex.Message;
            }
        }

        private async Task<bool> CheckIfNeedsRealTimeSearch(string age, string job, string income, string family)
        {
            // Simple heuristic: if asking about 2026 or specific current year data
            var currentYear = DateTime.Now.Year;
            var userInput = $"{age} {job} {income} {family}".ToLower();
            
            return userInput.Contains(currentYear.ToString()) || 
                   userInput.Contains("2026") || 
                   userInput.Contains("fiyat") || 
                   userInput.Contains("price") ||
                   userInput.Contains("güncel") ||
                   userInput.Contains("current");
        }

        /// <summary>
        /// Generates an automatic response to customer contact messages
        /// </summary>
        public async Task<string> GenerateContactAutoReply(string customerName, string subject, string message)
        {
            try
            {
                // Check if customer is asking about current prices or specific 2026 data
                var needsRealTimeData = message.ToLower().Contains("2026") || 
                                       message.ToLower().Contains("fiyat") || 
                                       message.ToLower().Contains("price") ||
                                       message.ToLower().Contains("güncel") ||
                                       message.ToLower().Contains("current") ||
                                       message.ToLower().Contains("latest");

                string contextInfo = "";
                if (needsRealTimeData)
                {
                    Debug.WriteLine("Customer asking for current data, searching web...");
                    var searchQuery = ExtractSearchQuery(message);
                    var searchResult = await _tavilyService.SearchWeb(searchQuery);
                    
                    if (searchResult.Success)
                    {
                        contextInfo = "\n\nReal-time market information:\n" + 
                                     _tavilyService.FormatInsuranceSearchResults(searchResult);
                    }
                }

                var prompt = $@"You are a professional customer service representative for LifeSure Insurance company.
A customer named {customerName} has sent a message with the following details:

Subject: {subject}
Message: {message}
{contextInfo}

Please generate a helpful, friendly, and professional response that:
1. Acknowledges their inquiry
2. If they asked for specific prices or 2026 data, use the real-time market information provided above
3. Provide SPECIFIC NUMBERS and PRICES if available from the search results
4. Provide relevant information about insurance services
5. Assures them that a team member will follow up soon
6. Keeps the response concise (3-4 paragraphs maximum)

Important: 
- If real market data is provided above, USE THOSE SPECIFIC PRICES in your answer
- Write in a warm, professional tone
- Be specific with numbers when available
Answer in English.";

                return await GenerateContent(prompt);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in GenerateContactAutoReply: " + ex.Message);
                return GetFallbackAutoReply(customerName);
            }
        }

        private string ExtractSearchQuery(string message)
        {
            // Extract key information from message for better search
            var query = message;
            
            // Add context for better search results
            if (message.ToLower().Contains("ev") || message.ToLower().Contains("home"))
                query += " home insurance policy prices Turkey";
            else if (message.ToLower().Contains("araba") || message.ToLower().Contains("car") || message.ToLower().Contains("auto"))
                query += " car insurance policy prices Turkey";
            else if (message.ToLower().Contains("saðlýk") || message.ToLower().Contains("health"))
                query += " health insurance policy prices Turkey";
            else if (message.ToLower().Contains("hayat") || message.ToLower().Contains("life"))
                query += " life insurance policy prices Turkey";
            else
                query += " insurance policy prices Turkey " + DateTime.Now.Year;

            return query;
        }

        private string GetFallbackAutoReply(string customerName)
        {
            return $@"Dear {customerName},

Thank you for reaching out to LifeSure Insurance. We have received your message and appreciate you taking the time to contact us.

Our dedicated team is reviewing your inquiry and will get back to you within 24-48 business hours. If you have any urgent questions, please don't hesitate to call us directly.

We look forward to assisting you with your insurance needs.

Best regards,
The LifeSure Insurance Team";
        }

        private async Task<string> GenerateContent(string prompt)
        {
            try
            {
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
                        maxOutputTokens = 2048
                    }
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Debug.WriteLine("Sending request to Gemini API...");
                
                var response = await _httpClient.PostAsync(url, content);
                var responseString = await response.Content.ReadAsStringAsync();

                Debug.WriteLine("Response Status: " + response.StatusCode.ToString());

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
