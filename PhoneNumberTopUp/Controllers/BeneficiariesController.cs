using Microsoft.AspNetCore.Mvc;
using PhoneNumberTopUp.Domain.Services;
using System.Net;

namespace PhoneNumberTopUp.API.Controllers;

// TODO: Auth
[ApiController]
[Route("topup")]
public class BeneficiariesController : ControllerBase
{
    public record BeneficiaryRecord(int PhoneNumber, string Nickname);

    private readonly IBeneficiaryService beneficiaryService;
    public BeneficiariesController(IBeneficiaryService beneficiaryService)
    {
        this.beneficiaryService = beneficiaryService;
    }

    [ProducesResponseType<List<BeneficiaryRecord>>((int)HttpStatusCode.OK)]
    [HttpGet("{userId}/beneficiaries")]
    public async Task<ActionResult<List<BeneficiaryRecord>>> Get(Guid userId, CancellationToken cancellationToken = default)
    {
        var beneficiaries = await beneficiaryService.GetBeneficiariesByUser(userId);

        return Ok(beneficiaries.Select(b => new BeneficiaryRecord(b.PhoneNumber, b.Nickname)));
    }

    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [HttpPost("{userId}/beneficiaries")]
    public async Task<ActionResult> AddBeneficiary(Guid userId, [FromBody] BeneficiaryRecord beneficiary, CancellationToken cancellationToken = default)
    {
        var result = await beneficiaryService.AddBeneficiary(userId, beneficiary.PhoneNumber, beneficiary.Nickname);

        return result switch
        {
            BeneficiaryOperationStatus.ServerError => StatusCode((int)HttpStatusCode.InternalServerError, beneficiary),
            BeneficiaryOperationStatus.ValidationError => StatusCode((int)HttpStatusCode.BadRequest, beneficiary),
            _ => Created()
        };
    }

    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [HttpDelete("{userId}/beneficiaries/{phonenumber}")]
    public async Task<ActionResult> DeleteBeneficiary(Guid userId, int phonenumber, CancellationToken cancellationToken = default)
    {
        var result = await beneficiaryService.DeleteBeneficiary(userId, phonenumber);

        return result switch
        {
            BeneficiaryOperationStatus.ServerError => StatusCode((int)HttpStatusCode.InternalServerError),
            BeneficiaryOperationStatus.BeneficiaryNotFound => StatusCode((int)HttpStatusCode.NotFound),
            _ => Ok()
        };
    }
}