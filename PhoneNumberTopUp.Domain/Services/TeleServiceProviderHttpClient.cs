using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace PhoneNumberTopUp.Domain.Services;

public class TeleServiceProviderHttpClient : ITeleServiceProviderHttpClient
{
    private int retryCount = 3;

    private readonly HttpClient httpClient;
    private readonly ILogger<TeleServiceProviderHttpClient> logger;

    public TeleServiceProviderHttpClient(HttpClient httpClient, ILogger<TeleServiceProviderHttpClient> logger)
    {
        this.httpClient = httpClient;
        this.logger = logger;
    }

    public async Task<bool> ExecuteTopUp(int senderPhoneNumber, int receiverPhoneNumber, int amount, string description, CancellationToken cancellationToken)
    {
        // By assuming the following to send request to a service provider.
        // - the request is a post.        
        // - add authorization some token into the header.
        // - response will be httpstatus code.        

        httpClient.DefaultRequestHeaders.Add("Authorization", "some token");

        var payload = JsonContent.Create(new
        {
            senderPhoneNumber,
            receiverPhoneNumber,
            amount,
            description
        });

        var httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            Content = payload
        };
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
                logger.LogError("ServiceProvider response code is {StatusCode}, and the reason is {ReasonPhrase}", response.StatusCode, response.ReasonPhrase);
                return false;
            }
        }

        return true;
    }
}
