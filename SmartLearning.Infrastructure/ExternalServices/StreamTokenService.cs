
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartLearning.Infrastructure.ExternalServices
{
    public class StreamTokenService : IStreamTokenService
    {
        private readonly string _apiKey;
        private readonly string _apiSecret;

        public StreamTokenService(IConfiguration config)
        {
            _apiKey = config["Stream:ApiKey"]!; 
            _apiSecret = config["Stream:ApiSecret"]!;
        }

        public string CreateUserToken(string userId, string callType, string callId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiSecret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var now = DateTimeOffset.UtcNow;
            var expires = now.AddHours(2);

            var token = new JwtSecurityToken(
                issuer: _apiKey,
                audience: null,
                claims: new[]
                {
                    new Claim("user_id", userId),
                    new Claim("call_cids", $"{callType}:{callId}")
                },
                notBefore: now.UtcDateTime,
                expires: expires.UtcDateTime,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}