using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies; // ✅ THIS LINE
using Azure.Identity;
using MerchStore.Application;
using MerchStore.Application.Common.Interfaces;
using MerchStore.Application.Services.Interfaces;
using MerchStore.Domain.Interfaces;
using MerchStore.Domain.Entities;
using MerchStore.Infrastructure;
using MerchStore.Infrastructure.ExternalServices;
using MerchStore.WebUI;
using MerchStore.WebUI.Authentication.ApiKey;
using MerchStore.WebUI.Infrastructure;



var builder = WebApplication.CreateBuilder(args);

// Load config
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .AddAzureKeyVault(new Uri("https://merchstorekeyvault123456.vault.azure.net/"), new DefaultAzureCredential());

// Add services
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy();
    options.JsonSerializerOptions.DictionaryKeyPolicy = new JsonSnakeCaseNamingPolicy();
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// 🔐 API Key
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "UserCookie"; // or "AdminCookie" depending on who logs in first
})
.AddCookie("UserCookie", options =>
{
    options.LoginPath = "/User/Login";
    options.AccessDeniedPath = "/User/Login";
})
.AddCookie("AdminCookie", options =>
{
    options.LoginPath = "/Admin/Login";
    options.AccessDeniedPath = "/Admin/AccessDenied";
})
.AddApiKey(builder.Configuration["ApiKey:Value"] ?? throw new InvalidOperationException("API Key not configured"));


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiKeyPolicy", policy =>
        policy.AddAuthenticationSchemes(ApiKeyAuthenticationDefaults.AuthenticationScheme).RequireAuthenticatedUser());

    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

// App Services
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebUI();
builder.Services.AddSingleton<MerchStore.Infrastructure.Storage.BlobStorageService>();

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// Debug which repository is in use
Console.WriteLine(builder.Configuration.GetValue<bool>("UseInMemoryDb")
    ? "🧪 Using In-Memory Product Repository"
    : "🌐 Using Azure Cosmos DB (Mongo API)");

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "MerchStore API",
        Version = "v1",
        Description = "API for MerchStore product catalog",
        Contact = new OpenApiContact
        {
            Name = "MerchStore Support",
            Email = "support@merchstore.1337.se"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath)) options.IncludeXmlComments(xmlPath);

    options.AddSecurityDefinition(ApiKeyAuthenticationDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = "API Key Authentication. Enter your API key.",
        Name = ApiKeyAuthenticationDefaults.HeaderName,
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = ApiKeyAuthenticationDefaults.AuthenticationScheme
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();

// 🌱 Seed Catalog
if (app.Environment.IsDevelopment() && !builder.Configuration.GetValue<bool>("UseInMemoryDb"))
{
    using var scope = app.Services.CreateScope();
    var seeder = scope.ServiceProvider.GetRequiredService<ICatalogSeeder>();
    await seeder.SeedAsync();
}

// 🛂 Ensure Admin User
using (var scope = app.Services.CreateScope())
{
    var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
    var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();

    var existingAdmin = await userRepo.GetByUsernameAsync("admin");
    if (existingAdmin == null)
    {
        await authService.RegisterUserAsync("admin", "admin123", "Admin");
        Console.WriteLine("✅ Admin user created!");
    }
    else
    {
        Console.WriteLine("ℹ️ Admin already exists.");
    }
}

// Middleware
if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (builder.Configuration.GetValue<bool>("EnableSwagger", true))
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MerchStore API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
