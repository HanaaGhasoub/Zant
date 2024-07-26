using Microsoft.EntityFrameworkCore;
using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Data.Repository;

public class BeneficiaryRepository : IBeneficiaryRepository
{
    private readonly TopUpDb topUpDb;

    public BeneficiaryRepository(TopUpDb topUpDb)
    {
        this.topUpDb = topUpDb;
    }

    public async Task<int> AddBeneficiary(Guid userId, int phoneNumber, string nickname)
    {
        var result = topUpDb.Beneficiarys.Add(new Beneficiary
        {
            UserId = userId,
            PhoneNumber = phoneNumber,
            Nickname = nickname
        });

        await topUpDb.SaveChangesAsync();

        return result.Entity.Id;
    }

    public async Task<List<Beneficiary>> GetBeneficiariesByUser(Guid id)
    {
        return await topUpDb.Beneficiarys.Where(b => b.UserId == id).ToListAsync();
    }

    public async Task<int> GetCountByUser(Guid id)
    {
        return await topUpDb.Beneficiarys.CountAsync(b => b.UserId == id);
    }
}
