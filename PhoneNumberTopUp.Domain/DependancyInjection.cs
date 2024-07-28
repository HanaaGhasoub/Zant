using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PhoneNumberTopUp.Data;
using PhoneNumberTopUp.Domain.Services;

namespace PhoneNumberTopUp.Domain;
public static class DependancyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataServices(configuration);

        services.AddScoped<ITopUpService, TopUpService>();
        services.AddScoped<IBeneficiaryService, BeneficiaryService>();
        services.AddScoped<ITopUpHandler, TopUpHandler>();

        services.AddScoped<IRealTimeHttpClient>(sp =>
        {
            var realTimeServiceUrl = configuration["RealTimeService"]!;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(realTimeServiceUrl)
            };

            return new RealTimeHttpClient(httpClient, sp.GetRequiredService<ILogger<RealTimeHttpClient>>());
        });
        services.AddScoped<ITeleServiceProviderHttpClient>(sp =>
        {
            var teleServiceProviderUrl = configuration["TeleServiceProvider"]!;

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(teleServiceProviderUrl)
            };

            return new TeleServiceProviderHttpClient(httpClient, sp.GetRequiredService<ILogger<TeleServiceProviderHttpClient>>());
        });

        return services;
    }
}
