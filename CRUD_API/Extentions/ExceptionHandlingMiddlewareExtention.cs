namespace CRUD_API.Extentions
{
    public static class ExceptionHandlingMiddlewareExtention
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CRUD_API.Middleware.ExceptionHandlingMiddleware>();
        }
    }
}
