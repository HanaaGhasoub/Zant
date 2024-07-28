using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Domain.Services;

public interface ITopUpService
{
    List<TopUpOption> GetTopUpOptions();

    Task<int> AddTransaction(Guid userId, int amount, int phoneNumber, int topUpCharges);

    /// <summary>
    /// check if the user's transactions limit has been reached per calendar month.
    /// </summary>
    /// <param name="user">user's uuid</param>
    /// <param name="phoneNumber">beneficiary's phone number</param>
    /// <param name="amount">transaction amount</param>
    /// <returns>True if the limit has not been reached, False otherwise.</returns>
    Task<bool> NewTransactionIsAllowed(User user, int phoneNumber, int amount);
}
