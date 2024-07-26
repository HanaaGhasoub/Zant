using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Domain.Services;

public interface IBeneficiaryService
{
    /// <summary>
    /// add new beneficiary to the user entity.
    /// </summary>
    /// <param name="userId">the user uuid</param>
    /// <param name="phoneNumber">the beneficiary phone number</param>
    /// <param name="nickname">the beneficiary nickname</param>
    /// <returns>AddBeneficiaryStatus enum indicates the result of the add beneficiary operation.</returns>
    Task<AddBeneficiaryStatus> AddBeneficiary(Guid userId, int phoneNumber, string nickname);

    /// <summary>
    /// get user's beneficiaries.
    /// </summary>
    /// <param name="userId">the user uuid</param>
    /// <returns>list of beneficiaries</returns>
    Task<List<Beneficiary>> GetBeneficiariesByUser(Guid userId);
}
