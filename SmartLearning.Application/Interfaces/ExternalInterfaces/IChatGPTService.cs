namespace SmartLearning.Application.Interfaces.ExternalInterfaces
{
    public interface IChatGPTService
    {
        Task<string> AskChatGPTAsync(string prompt);
    }
}
