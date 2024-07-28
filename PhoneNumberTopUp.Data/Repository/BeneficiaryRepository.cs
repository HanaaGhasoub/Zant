using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Data.Repository;

public class BeneficiaryRepository : IBeneficiaryRepository
{
    private readonly TopUpDb topUpDb;
    private readonly ILogger<BeneficiaryRepository> logger;

    public BeneficiaryRepository(TopUpDb topUpDb, ILogger<BeneficiaryRepository> logger)
    {
        this.topUpDb = topUpDb;
        this.logger = logger;
    }

    public async Task<int> AddBeneficiary(Guid userId, int phoneNumber, string nickname)
    {
        try
        {
            if (await InvalidBeneficiaryData(userId, phoneNumber, nickname))
            {
                return -1;
            }

            var result = topUpDb.Beneficiarys.Add(new Beneficiary
            {
                UserId = userId,
                PhoneNumber = phoneNumber,
                Nickname = nickname
            });

            return await topUpDb.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Empty);
            return 0;
        }
    }

    private async Task<bool> InvalidBeneficiaryData(Guid userId, int phoneNumber, string nickname)
    {
        //validate nickname length.
        if (nickname.Length > 20)
        {
            logger.LogWarning("Beneficiary nickname {PhoneNumber}-{Nickname} length should not be greater than 20 for user {UserId}.",
                phoneNumber, nickname, userId);
            return true;
        }

        var userBeneficiaries = await GetBeneficiariesByUser(userId);

        // validate max allowed beneficiary count.
        if (userBeneficiaries.Count == 5)
        {
            logger.LogWarning("User {UserId} has the max limit of adding beneficiary.", userId);
            return true;
        }

        // validate uniquness of beneficiary's phonenumber or nickname
        var phoneNumberIsUsed = userBeneficiaries.Any(b => b.PhoneNumber == phoneNumber);
        var nicknameIsUsed = userBeneficiaries.Any(b => b.Nickname == nickname);
        if (phoneNumberIsUsed || nicknameIsUsed)
        {
            logger.LogWarning("Violation of using same phonenumber/nickname with the same user.");
            return true;
        }

        return false;
    }

    public async Task<int> DeleteBeneficiary(Guid userId, int phoneNumber)
    {
        try
        {
            var userBeneficiaries = await GetBeneficiariesByUser(userId);

            var beneficiary = userBeneficiaries.FirstOrDefault(b => b.PhoneNumber == phoneNumber);

            if (beneficiary == null)
            {
                return -1;
            }

            beneficiary.Deleted = true;

            topUpDb.Beneficiarys.Update(beneficiary);

            return await topUpDb.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Empty);
            return 0;
        }
    }

    public async Task<List<Beneficiary>> GetBeneficiariesByUser(Guid userId)
    {
        return await topUpDb.Beneficiarys.Where(b => b.UserId == userId && !b.Deleted).ToListAsync();
    }
}
