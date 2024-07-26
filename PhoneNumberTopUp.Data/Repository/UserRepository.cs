using Microsoft.EntityFrameworkCore;
using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Data.Repository;

public class UserRepository : IUserRepository
{
    private readonly TopUpDb topUpDb;

    public UserRepository(TopUpDb topUpDb)
    {
        this.topUpDb = topUpDb;
    }

    public async Task<User?> Get(Guid id)
    {
        return await topUpDb.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}
