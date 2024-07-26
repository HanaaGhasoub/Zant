using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PhoneNumberTopUp.Data.Repository;

namespace PhoneNumberTopUp.Data;
public static class DependancyInjection
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TopUpDb>(options =>
           options.UseInMemoryDatabase(databaseName: "TopUpDb"));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBeneficiaryRepository, BeneficiaryRepository>();
        services.AddScoped<ITopUpRepository, TopUpRepository>();

        return services;
    }
}
