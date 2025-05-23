using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;

namespace MerchStore.WebUI.Infrastructure;
public class CultureMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _cultureName;

    public CultureMiddleware(RequestDelegate next, string cultureName)
    {
        _next = next;
        _cultureName = cultureName;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var cultureInfo = new CultureInfo(_cultureName);
        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}