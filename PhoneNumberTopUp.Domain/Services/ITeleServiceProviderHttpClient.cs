namespace PhoneNumberTopUp.Domain.Services;

public interface ITeleServiceProviderHttpClient
{
    Task<bool> ExecuteTopUp(int senderPhoneNumber, int receiverPhoneNumber, int amount, string description, CancellationToken cancellationToken);
}
