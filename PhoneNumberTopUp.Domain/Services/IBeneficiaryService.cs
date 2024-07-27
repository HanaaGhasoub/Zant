using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Domain.Services;

public interface IBeneficiaryService
{
    /// <summary>
    /// add new beneficiary to the user entity.
    /// </summary>
    /// <param name="userId">user uuid</param>
    /// <param name="phoneNumber">beneficiary phone number</param>
    /// <param name="nickname">beneficiary nickname</param>
    /// <returns>AddBeneficiaryStatus.Success if beneficiary is added successfully,
    /// AddBeneficiaryStatus.ServerError if database error is happened, AddBeneficiaryStatus.ValidationError if beneficiary's data invalid</returns>
    Task<BeneficiaryOperationStatus> AddBeneficiary(Guid userId, int phoneNumber, string nickname);

    /// <summary>
    /// get user's beneficiaries.
    /// </summary>
    /// <param name="userId">user uuid</param>
    /// <returns>User's beneficiaries</returns>
    Task<List<Beneficiary>> GetBeneficiariesByUser(Guid userId);

    /// <summary>
    /// remove user's beneficiary.
    /// </summary>
    /// <param name="userId">user uuid</param>
    /// <param name="phoneNumber">beneficiary phone number</param>
    /// <returns>True if removed successfully, False othewise</returns>
    Task<BeneficiaryOperationStatus> DeleteBeneficiary(Guid useId, int phoneNumber);
}
