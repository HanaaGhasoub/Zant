using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhoneNumberTopUp.Data;
using PhoneNumberTopUp.Domain.Services;

namespace PhoneNumberTopUp.Domain;
public static class DependancyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataServices(configuration);

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IBeneficiaryService, BeneficiaryService>();
        services.AddScoped<ITopUpService, TopUpService>();

        return services;
    }
}
