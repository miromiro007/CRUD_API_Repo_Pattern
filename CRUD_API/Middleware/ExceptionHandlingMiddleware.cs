namespace CRUD_API.Middleware
{
    public class ExceptionHandlingMiddleware
    {

        private readonly RequestDelegate _next; 

        public ExceptionHandlingMiddleware(RequestDelegate next) {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)    
            {

                Console.WriteLine($"Erreur capturée : {ex.Message}");

                context.Response.StatusCode = 500; // Internal Server Error
                context.Response.ContentType = "application/json";// Set the response content type to JSON
                var errorResponse = new
                {
                    Message = "Une erreur est survenue lors du traitement de la requête.",
                    Details = ex.Message
                };
                await context.Response.WriteAsJsonAsync(errorResponse);
            }
        }



    }
}
