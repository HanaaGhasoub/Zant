using Microsoft.Extensions.Logging;
using PhoneNumberTopUp.Data;
using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Domain.Services;

public class BeneficiaryService : IBeneficiaryService
{
    private readonly IBeneficiaryRepository beneficiaryRepository;
    private readonly ILogger<BeneficiaryService> logger;

    public BeneficiaryService(IBeneficiaryRepository beneficiaryRepository, ILogger<BeneficiaryService> logger)
    {
        this.beneficiaryRepository = beneficiaryRepository;
        this.logger = logger;
    }

    public async Task<BeneficiaryOperationStatus> AddBeneficiary(Guid userId, int phoneNumber, string nickname)
    {
        try
        {
            var newBeneficiaryId = await beneficiaryRepository.AddBeneficiary(userId, phoneNumber, nickname);

            return newBeneficiaryId switch
            {
                -1 => BeneficiaryOperationStatus.ValidationError,
                0 => BeneficiaryOperationStatus.ServerError,
                _ => BeneficiaryOperationStatus.BeneficiaryAdded,
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while adding beneficiary {PhoneNumber}-{Nickname} for user {UserId}.",
                phoneNumber, nickname, userId);

            return BeneficiaryOperationStatus.ServerError;
        }
    }

    public async Task<List<Beneficiary>> GetBeneficiariesByUser(Guid userId)
    {
        return await beneficiaryRepository.GetBeneficiariesByUser(userId);
    }

    public async Task<BeneficiaryOperationStatus> DeleteBeneficiary(Guid useId, int phoneNumber)
    {
        var result = await beneficiaryRepository.DeleteBeneficiary(useId, phoneNumber);

        return result switch
        {
            -1 => BeneficiaryOperationStatus.BeneficiaryNotFound,
            0 => BeneficiaryOperationStatus.ServerError,
            _ => BeneficiaryOperationStatus.BeneficiaryRemoved,
        };
    }

}
