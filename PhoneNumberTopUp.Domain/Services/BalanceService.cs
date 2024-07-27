namespace PhoneNumberTopUp.Domain.Services;

public class BalanaceService : IBalanaceService
{
    private readonly HttpClient httpClient;
    private readonly ServiceOptions serviceOptions;

    public BalanaceService(HttpClient httpClient, ServiceOptions serviceOptions)
    {
        this.httpClient = httpClient;
        this.serviceOptions = serviceOptions;
    }

    public async Task<float> GetBalance(Guid userId, CancellationToken cancellationToken)
    {
        // By assuming the follwing to get the balance
        // - the request is a post.
        // - userId is query param.
        // - add authorization some token into the header.
        // - response will be float type.

        httpClient.DefaultRequestHeaders.Add("Authorization", "some token");

        var response = await httpClient.PostAsync(serviceOptions.GetBalanceUrl, null, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return 0;
        }

        if (float.TryParse(await response.Content.ReadAsStringAsync(), out var balance))
        {
            return 0;
        }

        return balance;
    }
}
