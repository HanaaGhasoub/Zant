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

    [HttpPost]
    public ActionResult TopUp(TopUpRecord topUpBeneficiary)
    {
        //TODO: execute top - up transactions for user UAE phone numbers.

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var topUpOptions = await topUpService.GetTopUpOptions();

        return Ok(topUpOptions);
    }
}
