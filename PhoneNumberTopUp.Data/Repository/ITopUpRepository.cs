using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Data.Repository;

public interface ITopUpRepository
{
    Task<int> AddTransaction(Guid userId, int amount, int phoneNumber, int topUpCharges);

    List<TopUpOption> GetTopUpOptions();

    Task<List<TopUpTransaction>> GetTransactionsInThisMonth(Guid userId);
}
