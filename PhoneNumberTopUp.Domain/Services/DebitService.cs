namespace PhoneNumberTopUp.Domain.Services;

public class DebitService : IDebitService
{
    private readonly HttpClient httpClient;
    private readonly ServiceOptions serviceOptions;

    public DebitService(HttpClient httpClient, ServiceOptions serviceOptions)
    {
        this.httpClient = httpClient;
        this.serviceOptions = serviceOptions;
    }

    public async Task<bool> ExecuteDebitTransaction(Guid userId, int amount, string description, CancellationToken cancellationToken)
    {
        httpClient.DefaultRequestHeaders.Add("Authorization", "some token");

        var response = await httpClient.PostAsync(serviceOptions.CreateDebitUrl, null, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            // TODO: log response.
            return false;
        }

        return true;
    }
}
