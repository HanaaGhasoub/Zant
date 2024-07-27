using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Data.Repository;

public interface ITopUpRepository
{
    Task<TopUpTransaction?> AddTransaction(Guid userId, int amount, int phoneNumber, int topUpCharges);

    Task<List<TopUpOption>> GetTopUpOptions();

    Task<List<TopUpTransaction>> GetTransactionsInThisMonth(Guid userId);
}
