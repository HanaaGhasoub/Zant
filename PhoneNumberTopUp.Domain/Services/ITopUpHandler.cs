using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Domain.Services;

public interface ITopUpHandler
{
    Task<TopUpProcessStatus> Process(Guid userId, int phoneNumber, int amount, CancellationToken cancellationToken);
}
