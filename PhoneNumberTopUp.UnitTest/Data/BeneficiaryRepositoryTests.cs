using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using PhoneNumberTopUp.Data;
using PhoneNumberTopUp.Data.Repository;

namespace PhoneNumberTopUp.UnitTest.Data;

public class BeneficiaryRepositoryTests : DataTest
{
    private BeneficiaryRepository beneficiaryRepository;

    public BeneficiaryRepositoryTests()
    {
        var topUpDb = ServiceProvider.GetRequiredService<TopUpDb>();
        var logger = ServiceProvider.GetRequiredService<ILogger<BeneficiaryRepository>>();

        beneficiaryRepository = new BeneficiaryRepository(topUpDb, logger);
    }

    [Fact]
    public async Task AddBeneficiary_GivenValidData_ShouldReturnOne()
    {
        //arrange
        var userId = Fixture.Create<Guid>();
        var phoneNumber = Fixture.Create<int>();
        var beneficiaryName = "validnickname";

        //act
        var result = await beneficiaryRepository.AddBeneficiary(userId, phoneNumber, beneficiaryName);

        //assert
        result.Should().Be(1);
    }

    [Fact]
    public async Task AddBeneficiary_GivenInValidBeneficiaryNickname_ShouldReturnNegativeOne()
    {
        //arrange
        var userId = Fixture.Create<Guid>();
        var phoneNumber = Fixture.Create<int>();
        var beneficiaryName = "invalidbeneficiarylongnicknamelength";

        //act
        var result = await beneficiaryRepository.AddBeneficiary(userId, phoneNumber, beneficiaryName);

        //assert
        result.Should().Be(-1);
    }

    [Fact]
    public async Task AddBeneficiary_GivenUsedPhoneNumber_ShouldReturnNegativeOne()
    {
        //arrange
        var userId = Fixture.Create<Guid>();
        var phoneNumber = Fixture.Create<int>();
        var beneficiaryName = "beneficiary";

        await beneficiaryRepository.AddBeneficiary(userId, phoneNumber, beneficiaryName);

        var newPhoneNumber = Fixture.Create<int>();

        //act
        var result = await beneficiaryRepository.AddBeneficiary(userId, newPhoneNumber, beneficiaryName);

        //assert
        result.Should().Be(-1);
    }

    [Fact]
    public async Task AddBeneficiary_GivenUsedNickname_ShouldReturnNegativeOne()
    {
        //arrange
        var userId = Fixture.Create<Guid>();
        var phoneNumber = Fixture.Create<int>();
        var beneficiaryName = "beneficiary";

        await beneficiaryRepository.AddBeneficiary(userId, phoneNumber, beneficiaryName);

        var newNickname = "beneficiary";

        //act
        var result = await beneficiaryRepository.AddBeneficiary(userId, phoneNumber, newNickname);

        //assert
        result.Should().Be(-1);
    }

    [Fact]
    public async Task AddBeneficiary_GivenBeneficiaryMaxLimitAdding_ShouldReturnNegativeOne()
    {
        //arrange
        var userId = Fixture.Create<Guid>();
        for (int i = 0; i < 5; i++)
        {
            var phoneNumber = Fixture.Create<int>();
            var nickname = $"beneficiary{i}";

            await beneficiaryRepository.AddBeneficiary(userId, phoneNumber, nickname);
        }

        var newPhoneNumber = Fixture.Create<int>();
        var newNickname = "beneficiary6";

        //act
        var result = await beneficiaryRepository.AddBeneficiary(userId, newPhoneNumber, newNickname);

        //assert
        result.Should().Be(-1);
    }

    [Fact]
    public async Task AddBeneficiary_GivenDatabaseError_ShouldReturnZero()
    {
        //arrange
        var topUpDbMock = new Mock<TopUpDb>();
        topUpDbMock.Setup(s => s.SaveChangesAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());

        var logger = ServiceProvider.GetRequiredService<ILogger<BeneficiaryRepository>>();
        beneficiaryRepository = new BeneficiaryRepository(topUpDbMock.Object, logger);

        //act
        var userId = Fixture.Create<Guid>();
        var phoneNumber = Fixture.Create<int>();
        var beneficiaryName = "beneficiary";

        var result = await beneficiaryRepository.AddBeneficiary(userId, phoneNumber, beneficiaryName);

        //assert
        result.Should().Be(0);
    }    
}
