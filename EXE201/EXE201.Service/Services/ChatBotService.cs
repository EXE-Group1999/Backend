//using OpenAI;
//using Microsoft.Extensions.Configuration;
//using OpenAI.Chat;
//using System.ClientModel;
//using System.Threading;
//using System.Threading.Tasks;

//namespace EXE201.Service.Services
//{
//    public class ChatBotService
//    {
//        private readonly ChatClient _client;
//        private readonly string _model = "deepseek-r1-0528:free";
//        private const int MaxRetries = 3;
//        private const int InitialDelayMs = 1000;

//        public ChatBotService(IConfiguration configuration)
//        {
//            string apiKey = configuration["OpenAI:ApiKey"];
//            _client = new(_model, apiKey);
//        }

//        public async Task<string> GetChatResponseAsync(string userMessage)
//        {
//            if (string.IsNullOrWhiteSpace(userMessage))
//            {
//                throw new ArgumentException("User message cannot be empty", nameof(userMessage));
//            }

//            int attempt = 0;
//            while (attempt < MaxRetries)
//            {
//                try
//                {
//                    ChatCompletionOptions options = new()
//                    {
//                        MaxOutputTokenCount = 150,
//                        Temperature = 0.7f
//                    };

//                    ChatMessage[] messages = new[]
//                    {
//                        new UserChatMessage(userMessage)
//                    };

//                    ClientResult<ChatCompletion> result = await _client.CompleteChatAsync(messages, options);
//                    return result.Value.Content[0].Text;
//                }
//                catch (Exception ex) when (ex.Message.Contains("429"))
//                {
//                    attempt++;
//                    if (attempt >= MaxRetries)
//                    {
//                        throw new Exception("Max retries reached due to quota limit. Please check your OpenAI plan.", ex);
//                    }
//                    // Exponential backoff: 1s, 2s, 4s
//                    await Task.Delay(InitialDelayMs * (int)Math.Pow(2, attempt - 1));
//                }
//                catch (Exception ex)
//                {
//                    throw new Exception($"Error getting chat response: {ex.Message}", ex);
//                }
//            }

//            throw new Exception("Unexpected error in retry loop.");
//        }
//    }
//}
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace EXE201.Service.Services
{
    public class ChatBotService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _referer = "https://exe-api-dev-bcfpenbhf2f8a9cc.southeastasia-01.azurewebsites.net"; // Use your site or GitHub repo

        public ChatBotService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["OpenAI:ApiKey"];
        }

        public async Task<string> GetChatResponseAsync(string userMessage)
        {
            var requestBody = new
            {
                model = "deepseek/deepseek-r1-0528:free", // or "deepseek-coder", depending on the key
                messages = new[]
                {
                    new { role = "system", content = "Respond in English/Vietnamese only" },
                    new { role = "user", content = userMessage }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", _referer);

            var response = await _httpClient.PostAsync("https://openrouter.ai/api/v1/chat/completions", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"API Error: {response.StatusCode} - {error}");
            }

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var reply = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            return reply?.Trim();
        }
    }
}
