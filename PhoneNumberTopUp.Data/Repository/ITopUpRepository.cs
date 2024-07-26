using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Data.Repository;

public interface ITopUpRepository
{
    Task<List<TopUpOption>> GetTopUpOptions();
}
