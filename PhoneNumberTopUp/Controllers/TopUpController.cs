using Microsoft.AspNetCore.Mvc;
using PhoneNumberTopUp.Domain.Services;

namespace PhoneNumberTopUp.Controllers;

// TODO: Auth
[ApiController]
[Route("topup")]
public class TopUpController : ControllerBase
{
    public record TopUpRecord(Guid UserId, int PhoneNumber, int Amount);

    private readonly ITopUpService topUpService;

    public TopUpController(ITopUpService topUpService)
    {
        this.topUpService = topUpService;
    }

    [HttpPost("{userId}")]
    public async Task<ActionResult> TopUp([FromBody] TopUpRecord topUpRecord, CancellationToken cancellationToken = default)
    {
        await topUpService.Process(topUpRecord.UserId, topUpRecord.PhoneNumber, topUpRecord.Amount, cancellationToken);

        return Ok();
    }

    [HttpGet("{userId}/options")]
    public async Task<ActionResult> TopUpOptions(CancellationToken cancellationToken = default)
    {
        var topUpOptions = await topUpService.GetTopUpOptions();

        return Ok(topUpOptions);
    }
}
