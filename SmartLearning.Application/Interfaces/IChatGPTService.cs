
namespace SmartLearning.Application.Interfaces
{
    public interface IChatGPTService
    {
        Task<string> AskChatGPTAsync(string prompt);
    }
}
