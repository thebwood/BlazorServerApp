using BlazorApp.Web.Components;
using BlazorApp.Web.Services;
using BlazorApp.Web.Middleware;
using BlazorApp.Web.Extensions;
using MudBlazor.Services;
using Serilog;
using Microsoft.Extensions.Configuration;

// Configure Serilog from appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true)
        .Build())
    .Enrich.FromLogContext()
    .CreateLogger();

try
{
    Log.Information("Starting BlazorApp.Web application");

    var builder = WebApplication.CreateBuilder(args);

    // Add Serilog
    builder.Host.UseSerilog();

    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    builder.Services.AddMudServices();

    // Configure HttpClient for API calls with Resilience
    builder.Services.AddApiHttpClient<AddressService>("https://localhost:7208", acceptAnyCertificate: true);

    // Add Rate Limiting
    builder.Services.AddCustomRateLimiting();

    // Add Response Compression
    builder.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
    });

    // Add HTTP Logging
    builder.Services.AddHttpLogging(logging =>
    {
        logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
        logging.RequestHeaders.Add("X-Correlation-ID");
        logging.ResponseHeaders.Add("X-Correlation-ID");
    });

    var app = builder.Build();

    // Add Serilog request logging
    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    // IMPORTANT: Middleware order matters!

    // 1. Exception handling (should be first to catch all errors)
    if (!app.Environment.IsDevelopment())
    {
        app.UseGlobalExceptionHandler();
        app.UseHsts();
    }
    else
    {
        app.UseDeveloperExceptionPage();
    }

    // 2. Security headers
    app.UseSecurityHeaders();

    // 3. Correlation ID for request tracking
    app.UseCorrelationId();

    // 4. Request logging
    app.UseRequestLogging();

    // 5. Response compression
    app.UseResponseCompression();

    // 6. HTTP logging (development only)
    if (app.Environment.IsDevelopment())
    {
        app.UseHttpLogging();
    }

    // 7. Status code pages
    app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

    // 8. HTTPS redirection
    app.UseHttpsRedirection();

    // 9. Rate limiting
    app.UseRateLimiter();

    // 10. Antiforgery
    app.UseAntiforgery();

    // 11. Static files
    app.MapStaticAssets();

    // 12. Routing and endpoints
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
