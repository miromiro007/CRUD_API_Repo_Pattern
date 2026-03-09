namespace CRUD_API.Extentions
{
    public static class RequestLoggingMiddlewareExtension1
    {
        static public IApplicationBuilder UseRequestLogging1(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CRUD_API.Middleware.RequestLoggingMiddleware1>();
        }
    }
}
