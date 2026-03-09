namespace CRUD_API.Middleware
{

    // / <summary>
    /// Middleware to log incoming requests and outgoing responses.
    /// un exemple simple de middleware pour enregistrer les requêtes entrantes et les réponses sortantes.
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next; 
        
        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine("Request received");

            await _next(context);

            Console.Write("Response sent");
        }
    }
}
