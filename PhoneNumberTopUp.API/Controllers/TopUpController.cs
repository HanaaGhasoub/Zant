using Microsoft.AspNetCore.Mvc;
using PhoneNumberTopUp.Data.Entity;
using PhoneNumberTopUp.Domain.Services;
using System.Net;

namespace PhoneNumberTopUp.Controllers;

// TODO: Auth
[ApiController]
[Route("topup")]
public class TopUpController : ControllerBase
{
    public record TopUpRecord(Guid UserId, int PhoneNumber, int Amount);

    private readonly ITopUpService topUpService;
    private readonly ITopUpHandler topUpHandler;

    public TopUpController(ITopUpHandler topUpHandler, ITopUpService topUpService)
    {
        this.topUpHandler = topUpHandler;
        this.topUpService = topUpService;
    }

    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.PaymentRequired)]
    [ProducesResponseType((int)HttpStatusCode.RequestTimeout)]
    [ProducesResponseType((int)HttpStatusCode.NotAcceptable)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [HttpPost("{userId}")]
    public async Task<ActionResult> TopUp([FromBody] TopUpRecord topUpRecord, CancellationToken cancellationToken = default)
    {
        var topUpOptions = topUpService.GetTopUpOptions();
        if (!topUpOptions.Any(t => t.Value == topUpRecord.Amount))
        {
            return BadRequest(topUpRecord);
        }

        var status = await topUpHandler.Process(topUpRecord.UserId, topUpRecord.PhoneNumber, topUpRecord.Amount, cancellationToken);

        return status switch
        {
            TopUpProcessStatus.InsufficientBalance => StatusCode((int)HttpStatusCode.PaymentRequired),
            TopUpProcessStatus.UserNotFound => StatusCode((int)HttpStatusCode.NotFound),
            TopUpProcessStatus.ServiceProviderNotAvaliable => StatusCode((int)HttpStatusCode.RequestTimeout),
            TopUpProcessStatus.TransactionsLimitReached => StatusCode((int)HttpStatusCode.NotAcceptable),
            _ => Ok()
        };
    }

    [ProducesResponseType<List<TopUpOption>>((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [HttpGet("options")]
    public ActionResult<List<TopUpOption>> TopUpOptions(CancellationToken cancellationToken = default)
    {
        var topUpOptions = topUpService.GetTopUpOptions();

        return Ok(topUpOptions);
    }
}
