using System.Reflection;
using System.Text.Json.Serialization;
using MerchStore.Application;
using MerchStore.Infrastructure;
using MerchStore.WebUI.Authentication.ApiKey;
using MerchStore.WebUI.Infrastructure;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
    {
        // Use snake_case for JSON serialization
        options.JsonSerializerOptions.PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy();
        options.JsonSerializerOptions.DictionaryKeyPolicy = new JsonSnakeCaseNamingPolicy();
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Add API Key authentication
builder.Services.AddAuthentication()
   .AddApiKey(builder.Configuration["ApiKey:Value"] ?? throw new InvalidOperationException("API Key is not configured in the application settings."));

// Add API Key authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiKeyPolicy", policy =>
        policy.AddAuthenticationSchemes(ApiKeyAuthenticationDefaults.AuthenticationScheme)
              .RequireAuthenticatedUser());
});

// Add Application services - this includes Services, Interfaces, etc.
builder.Services.AddApplication();

// Add Infrastructure services - this includes DbContext, Repositories, etc.
builder.Services.AddInfrastructure(builder.Configuration);

// Add Swagger for API documentation
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

    // Include XML comments if you've enabled XML documentation in your project
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
    // Add API Key authentication support to Swagger UI
    options.AddSecurityDefinition(ApiKeyAuthenticationDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Description = "API Key Authentication. Enter your API key in the field below.",
        Name = ApiKeyAuthenticationDefaults.HeaderName,
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = ApiKeyAuthenticationDefaults.AuthenticationScheme
    });

    // Apply API key requirement only to controller-based endpoints
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();


// Add session support for cart management
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache(); // In-memory cache for session
builder.Services.AddHttpContextAccessor(); // Access to HttpContext in services 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // In development, seed the database with test data using the extension method
    app.Services.SeedDatabaseAsync().Wait();

    // Enable Swagger UI in development
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "MerchStore API V1");
    });
}

app.UseHttpsRedirection();
app.UseRouting();

// Enable session support
app.UseSession();

// Add authentication middleware
app.UseAuthentication();

// Add authorization middleware
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();