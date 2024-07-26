using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Domain.Services;

public interface IUserService
{
    Task<User?> GetUser(Guid id);
}
