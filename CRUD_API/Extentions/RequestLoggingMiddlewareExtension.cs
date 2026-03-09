using CRUD_API.Middleware;

namespace CRUD_API.Extentions
{
    // methode d'extension pour ajouter le middleware de logging des requêtes à la pipeline de traitement des requêtes HTTP.
    public static class RequestLoggingMiddlewareExtension
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExecutionTimeMiddleware>();
        }
    }
}
