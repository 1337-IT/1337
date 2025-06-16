using Microsoft.Extensions.DependencyInjection;

namespace MerchStore.WebUI;

public static class DependencyInjection
{
    public static IServiceCollection AddWebUI(this IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddSession(); // Required for cart sessions
        return services;
    }
}
