using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Data;

public interface IBeneficiaryRepository
{
    Task<int> AddBeneficiary(Guid userId, int phoneNumber, string nickname);

    Task<List<Beneficiary>> GetBeneficiariesByUser(Guid id);

    Task<int> GetCountByUser(Guid id);
}
