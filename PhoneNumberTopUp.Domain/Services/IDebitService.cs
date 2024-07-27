namespace PhoneNumberTopUp.Domain.Services;

public interface IDebitService
{
    Task<bool> ExecuteDebitTransaction(Guid userId, int amount, string description, CancellationToken cancellationToken);
}
