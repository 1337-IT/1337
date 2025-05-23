using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks; 

namespace MerchStore.WebUI.Infrastructure;

public static class CultureMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomCulture(this IApplicationBuilder builder, string cultureName)
    {
        return builder.UseMiddleware<CultureMiddleware>(cultureName);
    }
}