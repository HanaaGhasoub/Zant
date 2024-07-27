using PhoneNumberTopUp.Data.Entity;
using PhoneNumberTopUp.Data.Repository;

namespace PhoneNumberTopUp.Domain.Services;

public class TopUpService : ITopUpService
{
    private const int MaxTopUpAmountPerCalendarMonthPerBeneficiaryForNotVerifiedUser = 1000;
    private const int MaxTopUpAmountPerCalendarMonthPerBeneficiaryForVerifiedUser = 500;
    private const int MaxTopUpAmountPerCalendarMonthForVerifiedUser = 3000;

    private const int TopUpCharges = 1;

    private readonly ITopUpRepository topUpRepository;
    private readonly IUserRepository userRepository;
    private readonly IBalanaceService balanaceService;
    private readonly IDebitService debitService;

    public TopUpService(
        ITopUpRepository topUpRepository,
        IUserRepository userRepository,
        IBalanaceService balanaceService,
        IDebitService debitService)
    {
        this.topUpRepository = topUpRepository;
        this.userRepository = userRepository;
        this.balanaceService = balanaceService;
        this.debitService = debitService;
    }

    public async Task<List<TopUpOption>> GetTopUpOptions()
    {
        return await topUpRepository.GetTopUpOptions();
    }

    public async Task Process(Guid userId, int phoneNumber, int amount, CancellationToken cancellationToken)
    {
        var user = await userRepository.Get(userId);
        if (user == null)
        {
            return;
        }

        if (await UserAreNotAllowForNewTopUp(user, phoneNumber, amount))
        {
            return;
        }

        var transactionAmount = amount + TopUpCharges;

        // TODO: call external http service to get user balanace.
        var userBalance = await balanaceService.GetBalance(userId, cancellationToken);
        // TODO: validate the user has enough balanace.
        if (userBalance < transactionAmount)
        {
            return;
        }

        // TODO: call external http service to post debit transaction
        var debitTransactionResponse = await debitService
            .ExecuteDebitTransaction(userId, transactionAmount, $"Top up phone number {phoneNumber}", cancellationToken);
        if (!debitTransactionResponse)
        {
            return;
        }

        var transaction = await topUpRepository.AddTransaction(userId, amount, phoneNumber, TopUpCharges);
        if (transaction == null)
        {
            //TODO: retry logic

            //TODO: if retry does not work as well, then log.
            return;
        }
    }

    private async Task<bool> UserAreNotAllowForNewTopUp(User user, int phoneNumber, int amount)
    {
        var transactionsInThisMonth = await topUpRepository.GetTransactionsInThisMonth(user.Id);

        //total topup amounts in this months for all the beneficiaries.
        var totalTopUpAmountInThisMonth = transactionsInThisMonth.Sum(x => x.Amount);

        var newtotalTopUpAmountInThisMonth = totalTopUpAmountInThisMonth + amount;

        //user cannot topup more than 3000 per calendar month per all beneficiaries.
        if (newtotalTopUpAmountInThisMonth > MaxTopUpAmountPerCalendarMonthForVerifiedUser)
        {
            return true;
        }

        //total topup amounts in this months for one beneficiary provided.
        var totalTopUpAmountInThisMonthPerBeneficiary = transactionsInThisMonth
            .Where(t => t.PhoneNumber == phoneNumber)
            .Sum(x => x.Amount);

        var newtotalTopUpAmountInThisMonthPerBeneficiary = totalTopUpAmountInThisMonthPerBeneficiary + amount;

        //verified user cannot topup more than 500 per calendar month per beneficiary.
        if (user.Verified && newtotalTopUpAmountInThisMonthPerBeneficiary > MaxTopUpAmountPerCalendarMonthPerBeneficiaryForVerifiedUser)
        {
            return true;
        }

        //non-verified user cannot topup more than 1000 per calendar month per beneficiary.
        if (!user.Verified && newtotalTopUpAmountInThisMonthPerBeneficiary > MaxTopUpAmountPerCalendarMonthPerBeneficiaryForNotVerifiedUser)
        {
            return true;
        }

        return false;
    }
}
