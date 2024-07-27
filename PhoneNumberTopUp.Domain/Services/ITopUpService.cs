using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Domain.Services;

public interface ITopUpService
{
    Task<List<TopUpOption>> GetTopUpOptions();

    Task Process(Guid userId, int phoneNumber, int amount, CancellationToken cancellationToken);
}
