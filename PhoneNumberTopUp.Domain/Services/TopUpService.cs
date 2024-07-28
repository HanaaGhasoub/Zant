using Microsoft.Extensions.Logging;
using PhoneNumberTopUp.Data.Entity;
using PhoneNumberTopUp.Data.Repository;

namespace PhoneNumberTopUp.Domain.Services;

public class TopUpService : ITopUpService
{
    private const int MaxTopUpAmountPerCalendarMonthPerBeneficiaryForNotVerifiedUser = 1000;
    private const int MaxTopUpAmountPerCalendarMonthPerBeneficiaryForVerifiedUser = 500;
    private const int MaxTopUpAmountPerCalendarMonthForVerifiedUser = 3000;

    private readonly ITopUpRepository topUpRepository;
    private readonly ILogger<TopUpService> logger;

    public TopUpService(ITopUpRepository topUpRepository, ILogger<TopUpService> logger)
    {
        this.topUpRepository = topUpRepository;
        this.logger = logger;
    }

    public async Task<int> AddTransaction(Guid userId, int amount, int phoneNumber, int topUpCharges)
    {
        return await topUpRepository.AddTransaction(userId, amount, phoneNumber, topUpCharges);
    }

    public List<TopUpOption> GetTopUpOptions()
    {
        return topUpRepository.GetTopUpOptions();
    }

    public async Task<bool> NewTransactionIsAllowed(User user, int phoneNumber, int amount)
    {
        var transactionsInThisMonth = await topUpRepository.GetTransactionsInThisMonth(user.Id);

        //total topup amounts in this months for all the beneficiaries.
        var totalTopUpAmountInThisMonth = transactionsInThisMonth.Sum(x => x.Amount);

        var newtotalTopUpAmountInThisMonth = totalTopUpAmountInThisMonth + amount;

        //user cannot topup more than 3000 per calendar month per all beneficiaries.
        if (newtotalTopUpAmountInThisMonth > MaxTopUpAmountPerCalendarMonthForVerifiedUser)
        {
            return false;
        }

        //total topup amounts in this months for one beneficiary provided.
        var totalTopUpAmountInThisMonthPerBeneficiary = transactionsInThisMonth
            .Where(t => t.PhoneNumber == phoneNumber)
            .Sum(x => x.Amount);

        var newtotalTopUpAmountInThisMonthPerBeneficiary = totalTopUpAmountInThisMonthPerBeneficiary + amount;

        //verified user cannot topup more than 500 per calendar month per beneficiary.
        if (user.Verified && newtotalTopUpAmountInThisMonthPerBeneficiary > MaxTopUpAmountPerCalendarMonthPerBeneficiaryForVerifiedUser)
        {
            return false;
        }

        //non-verified user cannot topup more than 1000 per calendar month per beneficiary.
        if (!user.Verified && newtotalTopUpAmountInThisMonthPerBeneficiary > MaxTopUpAmountPerCalendarMonthPerBeneficiaryForNotVerifiedUser)
        {
            return false;
        }

        return true;
    }
}
