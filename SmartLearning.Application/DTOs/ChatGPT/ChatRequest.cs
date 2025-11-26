
namespace SmartLearning.Application.DTOs.ChatGPT
{
    public class ChatRequest
    {
        public List<ChatGPTMessage> Messages { get; set; } = new List<ChatGPTMessage>();
    }
}
