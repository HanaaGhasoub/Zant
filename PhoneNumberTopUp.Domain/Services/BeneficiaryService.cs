using Microsoft.Extensions.Logging;
using PhoneNumberTopUp.Data;
using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Domain.Services;

public class BeneficiaryService : IBeneficiaryService
{
    private const uint MAXBENEFICIARIELIMIT = 5;
    private const uint MAXBENEFICIARYNICKNAMELENGTH = 20;

    private readonly IBeneficiaryRepository beneficiaryRepository;
    private readonly ILogger<BeneficiaryService> logger;

    public BeneficiaryService(IBeneficiaryRepository beneficiaryRepository, ILogger<BeneficiaryService> logger)
    {
        this.beneficiaryRepository = beneficiaryRepository;
        this.logger = logger;
    }

    public async Task<AddBeneficiaryStatus> AddBeneficiary(Guid userId, int phoneNumber, string nickname)
    {
        try
        {
            //validate nickname length.
            if (nickname.Length > MAXBENEFICIARYNICKNAMELENGTH)
            {
                logger.LogWarning("Beneficiary nickname {PhoneNumber}-{Nickname} length is greater than {MAXBENEFICIARYNICKNAMELENGTH} for user {UserId}.",
                    phoneNumber, nickname, MAXBENEFICIARYNICKNAMELENGTH, userId);

                return AddBeneficiaryStatus.InvalidLength;
            }

            //validate beneficiaries max limit is reached.
            var userBeneficiaryCount = await beneficiaryRepository.GetCountByUser(userId);
            if (userBeneficiaryCount == MAXBENEFICIARIELIMIT)
            {
                logger.LogWarning("User {UserId} has the max limit of adding beneficiary.", userId);

                return AddBeneficiaryStatus.MaxLimitReached;
            }

            var newBeneficiaryId = await beneficiaryRepository.AddBeneficiary(userId, phoneNumber, nickname);

            return newBeneficiaryId > 0 ? AddBeneficiaryStatus.Success : AddBeneficiaryStatus.Error;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while adding beneficiary {PhoneNumber}-{Nickname} for user {UserId}.",
                phoneNumber, nickname, userId);

            return AddBeneficiaryStatus.Error;
        }
    }

    public async Task<List<Beneficiary>> GetBeneficiariesByUser(Guid userId)
    {
        return await beneficiaryRepository.GetBeneficiariesByUser(userId);
    }
}
