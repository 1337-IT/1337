using Microsoft.Extensions.DependencyInjection;
using MerchStore.Application.Services.Interfaces;
using MerchStore.Application.Services.Implementations;

namespace MerchStore.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICatalogService, CatalogService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IReviewService, ReviewService>(); // ✅ Add this
        return services;
    }
}
