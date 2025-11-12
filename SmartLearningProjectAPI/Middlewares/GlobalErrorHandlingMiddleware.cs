
namespace SmartLearningProjectAPI.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalErrorHandlingMiddleware> _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception caught by GlobalErrorHandlingMiddleware at {Path}", context.Request.Path);

                var statusCode = ex switch
                {
                    ArgumentException => HttpStatusCode.BadRequest,
                    UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                    _ => HttpStatusCode.InternalServerError
                };

                var errorResponse = new
                {
                    message = ex.Message,
                    statusCode = (int)statusCode
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;

                var json = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(json);
            }
        }
    }
}