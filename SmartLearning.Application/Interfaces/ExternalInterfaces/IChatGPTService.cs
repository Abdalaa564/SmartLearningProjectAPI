using SmartLearning.Application.DTOs.ChatGPT;

namespace SmartLearning.Application.Interfaces.ExternalInterfaces
{
    public interface IChatGPTService
    {
        //Task<string> AskChatGPTAsync(string prompt);
        Task<string> AskChatGPTAsync(List<ChatGPTMessage> messages);
    }
}
