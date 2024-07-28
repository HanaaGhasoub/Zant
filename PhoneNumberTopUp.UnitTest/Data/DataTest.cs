using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PhoneNumberTopUp.Data;
using PhoneNumberTopUp.Data.Repository;

namespace PhoneNumberTopUp.UnitTest.Data;

public class DataTest
{
    private readonly IServiceCollection serviceCollection = new ServiceCollection();
    protected readonly IServiceProvider ServiceProvider;

    protected readonly Fixture Fixture;

    public DataTest()
    {
        Fixture = new Fixture();

        var configuration = new ConfigurationBuilder().Build();

        serviceCollection.AddDataServices(configuration).AddLogging();

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    [Theory]
    [InlineData(typeof(IBeneficiaryRepository), typeof(BeneficiaryRepository))]
    [InlineData(typeof(ITopUpRepository), typeof(TopUpRepository))]
    [InlineData(typeof(IUserRepository), typeof(UserRepository))]
    [InlineData(typeof(TopUpDb), typeof(TopUpDb))]
    public void ShouldRegisterAllDataServices(Type serviceType, Type implementationType)
    {
        //act        
        var serviceDescriptor = serviceCollection.SingleOrDefault(
            d => d.ServiceType == serviceType &&
                 d.ImplementationType == implementationType);
        //assert
        serviceDescriptor.Should().NotBeNull();
    }
}
