using Microsoft.EntityFrameworkCore;
using PhoneNumberTopUp.Data;
using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Domain.Services;

public class UserService : IUserService
{
    private readonly TopUpDb _topUpDb;

    public UserService(TopUpDb topUpDb)
    {
        _topUpDb = topUpDb;
    }

    public async Task<User?> GetUser(Guid id)
    {
        return await _topUpDb.Users.FirstOrDefaultAsync(x => x.Id == id);
    }
}
