using System.Diagnostics;

namespace CRUD_API.Middleware
{
    public class ExecutionTimeMiddleware
    {

        private readonly RequestDelegate _next;
        public ExecutionTimeMiddleware(RequestDelegate next)
        {
                       _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Request begin");
            await  _next(context);

            stopwatch.Stop();

            Console.WriteLine($" end of requestion time : {stopwatch.ElapsedMilliseconds} ms ");

        }
    }
}
