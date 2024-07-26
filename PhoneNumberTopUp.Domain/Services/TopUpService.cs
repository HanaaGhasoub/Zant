using PhoneNumberTopUp.Data.Entity;
using PhoneNumberTopUp.Data.Repository;

namespace PhoneNumberTopUp.Domain.Services;

public class TopUpService : ITopUpService
{
    private readonly ITopUpRepository topUpRepository;

    public TopUpService(ITopUpRepository topUpRepository)
    {
        this.topUpRepository = topUpRepository;
    }

    public async Task<List<TopUpOption>> GetTopUpOptions()
    {
        return await topUpRepository.GetTopUpOptions();
    }
}
