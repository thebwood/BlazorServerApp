namespace BlazorApp.Web.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddApiHttpClient<TClient>(
        this IServiceCollection services,
        string baseAddress,
        bool acceptAnyCertificate = false) where TClient : class
    {
        var httpClientBuilder = services.AddHttpClient<TClient>(client =>
        {
            client.BaseAddress = new Uri(baseAddress);
        });

        if (acceptAnyCertificate)
        {
            httpClientBuilder.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            });
        }

        httpClientBuilder.AddStandardResilienceHandler();

        return services;
    }
}
