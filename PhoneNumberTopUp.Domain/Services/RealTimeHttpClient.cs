using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;

namespace PhoneNumberTopUp.Domain.Services;

public class RealTimeHttpClient : IRealTimeHttpClient
{
    private int retryCount = 3;

    private readonly HttpClient httpClient;
    private readonly ILogger<RealTimeHttpClient> logger;

    public RealTimeHttpClient(HttpClient httpClient, ILogger<RealTimeHttpClient> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public async Task<float> GetBalance(Guid userId, CancellationToken cancellationToken)
    {
        // By assuming the follwing to get the balance
        // - the request is a post.
        // - userId is query param.
        // - add authorization some token into the header.
        // - response will be float type.        

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, string.Empty);
        httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("schema", "some token");

        while (retryCount > 0)
        {
            var response = await httpClient.SendAsync(httpRequestMessage);

            if (response.StatusCode == HttpStatusCode.RequestTimeout ||
              response.StatusCode == HttpStatusCode.ServiceUnavailable)
            {
                retryCount--;
                continue;
            }

            if (!response.IsSuccessStatusCode)
            {
                logger.LogError("RealTimeService response code is {StatusCode}, and the reason is {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
                return 0;
            }

            var content = await response.Content.ReadAsStringAsync();
            if (float.TryParse(content, out var balance))
            {
                logger.LogError("RealTimeService could not parse the response {content} into float.", content);
                return 0;
            }

            return balance;
        }

        return 0;
    }
}
