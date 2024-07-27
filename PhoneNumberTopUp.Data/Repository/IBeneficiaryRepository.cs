using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Data;

public interface IBeneficiaryRepository
{
    /// <summary>
    /// Add new beneficiary to a user.
    /// </summary>
    /// <param name="userId">user uuid</param>
    /// <param name="phoneNumber">beneficiary's phonenumber</param>
    /// <param name="nickname">beneficiary's nickname</param>
    /// <returns>1 when added successfully, -1 when validation error, and 0 when database error</returns>
    Task<int> AddBeneficiary(Guid userId, int phoneNumber, string nickname);

    /// <summary>
    /// Hard delete a beneficiary of a user. 
    /// Soft delete will require extra changes to the beneficiary entity
    /// </summary>
    /// <param name="userId">user uuid</param>
    /// <param name="phoneNumber">beneficiary's phonenumber</param>
    /// <returns>1 when deleted successfully, -1 when beneficiary is not found, and 0 when database error </returns>
    Task<int> DeleteBeneficiary(Guid userId, int phoneNumber);

    /// <summary>
    /// Fetch all user's beneficiaries.
    /// </summary>
    /// <param name="userId">user uuid</param>
    /// <returns>list of a user's beneficiaries</returns>
    Task<List<Beneficiary>> GetBeneficiariesByUser(Guid userId);
}
