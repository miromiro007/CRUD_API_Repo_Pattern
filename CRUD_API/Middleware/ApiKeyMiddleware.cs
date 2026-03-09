namespace CRUD_API.Middleware
{
    public class ApiKeyMiddleware
    {
        // ce middleware vérifie la présence d'une clé API dans les en-têtes de la requête et valide sa valeur. Si la clé est manquante ou incorrecte, il renvoie une réponse 401 Unauthorized. Sinon, il permet à la requête de continuer vers le prochain middleware ou le contrôleur.
        private readonly RequestDelegate _next;
        private const string APIKEYHEADERNAME = "X-API-KEY";
        private const string APIKEYVALUE = "123456";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(APIKEYHEADERNAME, out var extractedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                Console.WriteLine("API Key is missing");
                await context.Response.WriteAsync("API Key is missing");
                return;
            }
            if (extractedApiKey != APIKEYVALUE)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                Console.WriteLine("Unauthorized client");
                await context.Response.WriteAsync("Unauthorized client");
                return;
            }

            await _next(context);
        }
    }
}
