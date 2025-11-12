
namespace SmartLearningProjectAPI.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly Stopwatch _stopwatch;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
            _stopwatch = new Stopwatch();
        }
        public async Task InvokeAsync(HttpContext context)
        {
            _stopwatch.Restart();

            Log.Information("➡️ Incoming Request: {Method} {Path}",
                context.Request.Method, context.Request.Path);

            try
            {
                await _next(context);

                _stopwatch.Stop();

                Log.Information("Completed {Method} {Path} with {StatusCode} in {Elapsed} ms",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    _stopwatch.Elapsed.TotalSeconds.ToString("0.00"));
            }
            catch (Exception ex)
            {
                _stopwatch.Stop();

                Log.Error(ex, "Error handling {Method} {Path} after {Elapsed} ms",
                    context.Request.Method,
                    context.Request.Path,
                    _stopwatch.ElapsedMilliseconds);

                throw;
            }
        }
    }
}
