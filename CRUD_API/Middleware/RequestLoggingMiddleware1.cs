namespace CRUD_API.Middleware
{
    public class RequestLoggingMiddleware1
    {
        private readonly RequestDelegate _next; 

        public RequestLoggingMiddleware1(RequestDelegate next)
        {             _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine($"Request received: {context.Request.Method} / {context.Request.Path}");
            await _next(context);
            Console.WriteLine($"Response sent : {context.Response.StatusCode} / {context.Response.Body}"); 
        }
    }
}
