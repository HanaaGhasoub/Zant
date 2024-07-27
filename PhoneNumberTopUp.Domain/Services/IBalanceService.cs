namespace PhoneNumberTopUp.Domain.Services;

public interface IBalanaceService
{
    Task<float> GetBalance(Guid userId, CancellationToken cancellationToken);
}
