using Microsoft.AspNetCore.Mvc;
using PhoneNumberTopUp.Domain.Services;
using System.Net;

namespace PhoneNumberTopUp.API.Controllers;

// TODO: Auth
[ApiController]
[Route("topup/beneficiaries")]
public class BeneficiariesController : ControllerBase
{
    public record BeneficiaryRecord(int PhoneNumber, string Nickname);

    private readonly IBeneficiaryService beneficiaryService;
    public BeneficiariesController(IBeneficiaryService beneficiaryService)
    {
        this.beneficiaryService = beneficiaryService;
    }

    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotAcceptable)]
    [ProducesResponseType((int)HttpStatusCode.ExpectationFailed)]
    [HttpPost("{userId}")]
    public async Task<ActionResult> Post(Guid userId, [FromBody] BeneficiaryRecord beneficiary)
    {
        var result = await beneficiaryService.AddBeneficiary(userId, beneficiary.PhoneNumber, beneficiary.Nickname);

        return result switch
        {
            AddBeneficiaryStatus.Success => Created(),
            AddBeneficiaryStatus.Error => StatusCode((int)HttpStatusCode.InternalServerError, beneficiary),
            AddBeneficiaryStatus.InvalidLength => StatusCode((int)HttpStatusCode.BadRequest, beneficiary),
            AddBeneficiaryStatus.MaxLimitReached => StatusCode((int)HttpStatusCode.NotAcceptable, beneficiary),
            _ => StatusCode((int)HttpStatusCode.ExpectationFailed, beneficiary)
        };
    }

    [ProducesResponseType<List<BeneficiaryRecord>>((int)HttpStatusCode.OK)]
    [HttpGet("{userId}")]
    public async Task<ActionResult<List<BeneficiaryRecord>>> Get(Guid userId)
    {
        var beneficiaries = await beneficiaryService.GetBeneficiariesByUser(userId);

        return Ok(beneficiaries.Select(b => new BeneficiaryRecord(b.PhoneNumber, b.Nickname)));
    }
}