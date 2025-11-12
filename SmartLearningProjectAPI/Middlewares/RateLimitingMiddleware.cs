
namespace SmartLearningProjectAPI.Middlewares
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;

        private static readonly ConcurrentDictionary<string, (DateTime Timestamp, int Count)> _requests = new();

        private readonly int _limit = 5;
        private readonly TimeSpan _window = TimeSpan.FromSeconds(10);
        
        public RateLimitingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            var now = DateTime.UtcNow;
            var entry = _requests.GetOrAdd(ipAddress, _ => (now, 0));

            if (now - entry.Timestamp < _window)
            {
                if (entry.Count >= _limit)
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.Response.WriteAsync("Too many requests. Please try again later.");
                    return;
                }

                _requests[ipAddress] = (entry.Timestamp, entry.Count + 1);
            }
            else
            {
                _requests[ipAddress] = (now, 1);
            }

            await _next(context);
        }
    }
}