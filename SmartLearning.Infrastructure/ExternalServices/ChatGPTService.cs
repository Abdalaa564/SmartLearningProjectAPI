



namespace SmartLearning.Infrastructure.ExternalServices
{
    public class ChatGPTService : IChatGPTService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ChatGPTService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<string> AskChatGPTAsync(string prompt)
        {
            var apiKey = _config["OpenAI:ApiKey"];

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            var request = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                new { role = "user", content = prompt }
            }
            };

            var response = await _httpClient.PostAsJsonAsync(
                "https://api.openai.com/v1/chat/completions", request);

            if (!response.IsSuccessStatusCode)
                return $"Error: {response.StatusCode}";

            var json = await response.Content.ReadFromJsonAsync<ChatGPTResponse>();
            return json?.choices?.FirstOrDefault()?.message?.content ?? "No response from ChatGPT.";
        }
    }
}
