using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Domain.Services;

public interface ITopUpService
{
    Task<List<TopUpOption>> GetTopUpOptions();

    //TODO: Process top-up
}
