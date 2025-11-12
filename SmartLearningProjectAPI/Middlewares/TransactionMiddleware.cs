
namespace SmartLearningProjectAPI.Middlewares
{
    public class TransactionMiddleware
    {
        private readonly RequestDelegate _next;

        public TransactionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ITIEntity dbContext)
        {
            var method = context.Request.Method;

            if (method == HttpMethods.Post || method == HttpMethods.Put || method == HttpMethods.Delete)
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync();

                try
                {
                    await _next(context);

                    await dbContext.Database.CommitTransactionAsync();
                }
                catch
                {
                    await dbContext.Database.RollbackTransactionAsync();
                    throw;
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
