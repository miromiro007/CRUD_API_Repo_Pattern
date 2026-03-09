namespace CRUD_API.Extentions
{
    public static class ApiKeyMiddlewareExtention
    {
        static public IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CRUD_API.Middleware.ApiKeyMiddleware>();
        }
    }
}
