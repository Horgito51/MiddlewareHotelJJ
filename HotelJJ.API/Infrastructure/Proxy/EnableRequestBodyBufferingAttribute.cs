using Microsoft.AspNetCore.Mvc.Filters;

namespace HotelJJ.API.Infrastructure.Proxy;

public sealed class EnableRequestBodyBufferingAttribute : Attribute, IResourceFilter
{
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
        if (HasBody(context.HttpContext.Request.Method))
        {
            context.HttpContext.Request.EnableBuffering();
        }
    }

    public void OnResourceExecuted(ResourceExecutedContext context)
    {
    }

    private static bool HasBody(string method)
    {
        return method.Equals(HttpMethods.Post, StringComparison.OrdinalIgnoreCase)
            || method.Equals(HttpMethods.Put, StringComparison.OrdinalIgnoreCase)
            || method.Equals(HttpMethods.Patch, StringComparison.OrdinalIgnoreCase);
    }
}
