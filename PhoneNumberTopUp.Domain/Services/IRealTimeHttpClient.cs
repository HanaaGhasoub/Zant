namespace PhoneNumberTopUp.Domain.Services;

public interface IRealTimeHttpClient
{
    /// <summary>
    /// get a user real-time balanace from external http service.
    /// </summary>
    /// <param name="userId">user uuid</param>
    /// <param name="cancellationToken"></param>
    /// <returns>Returns user's balance.</returns>
    Task<float> GetBalance(Guid userId, CancellationToken cancellationToken);
}
