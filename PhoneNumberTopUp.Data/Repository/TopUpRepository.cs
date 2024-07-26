using Microsoft.EntityFrameworkCore;
using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Data.Repository;

public class TopUpRepository : ITopUpRepository
{
    private readonly TopUpDb topUpDb;

    public TopUpRepository(TopUpDb topUpDb)
    {
        this.topUpDb = topUpDb;
    }

    public async Task<List<TopUpOption>> GetTopUpOptions()
    {
        return await topUpDb.TopUpOptions.ToListAsync();
    }
}
