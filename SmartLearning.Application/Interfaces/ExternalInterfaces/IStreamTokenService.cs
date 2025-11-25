
namespace SmartLearning.Application.Interfaces.ExternalInterfaces
{
    public interface IStreamTokenService
    {
        string CreateUserToken(string userId, string callType, string callId);
    }
}
