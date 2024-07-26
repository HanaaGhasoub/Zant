using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Data.Repository;

public interface IUserRepository
{
    /// <summary>
    /// get user by id
    /// </summary>
    /// <param name="id">user uuid</param>
    /// <returns>user entity or null if not exist</returns>
    Task<User?> Get(Guid id);
}
