
namespace SmartLearning.Application.Interfaces.ExternalInterfaces
{
    public interface IPdfChatService
    {
        Task<string> ExtractTextAsync(IFormFile file);
    }
}
