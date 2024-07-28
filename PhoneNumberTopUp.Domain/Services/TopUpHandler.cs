using PhoneNumberTopUp.Data.Repository;

namespace PhoneNumberTopUp.Domain.Services;

public class TopUpHandler : ITopUpHandler
{
    private const int TopUpCharges = 1;

    private readonly IUserRepository userRepository;
    private readonly ITopUpService transactionService;
    private readonly IRealTimeHttpClient realTimeHttpClient;
    private readonly ITeleServiceProviderHttpClient teleServiceProviderHttpClient;

    public TopUpHandler(
        IUserRepository userRepository,
        ITopUpService transactionService,
        IRealTimeHttpClient realTimeHttpClient,
        ITeleServiceProviderHttpClient teleServiceProviderHttpClient)
    {
        this.userRepository = userRepository;
        this.transactionService = transactionService;
        this.realTimeHttpClient = realTimeHttpClient;
        this.teleServiceProviderHttpClient = teleServiceProviderHttpClient;
    }

    public async Task<TopUpProcessStatus> Process(Guid userId, int phoneNumber, int amount, CancellationToken cancellationToken)
    {
        var user = await userRepository.Get(userId);
        if (user == null)
        {
            return TopUpProcessStatus.UserNotFound;
        }

        //check user's transactions limit per calendar month.
        var userAllowedForNewTransaction = await transactionService.NewTransactionIsAllowed(user, phoneNumber, amount);
        if (userAllowedForNewTransaction == false)
        {
            return TopUpProcessStatus.TransactionsLimitReached;
        }

        var transactionAmount = amount + TopUpCharges;

        //check the user's balance.
        var userBalance = await realTimeHttpClient.GetBalance(userId, cancellationToken);
        if (userBalance < transactionAmount)
        {
            return TopUpProcessStatus.InsufficientBalance;
        }

        //execute the topup transaction through external service provider.
        var topUpResponseStatus = await teleServiceProviderHttpClient
            .ExecuteTopUp(user.PhoneNumber, phoneNumber, transactionAmount, $"Top up phone number {phoneNumber}", cancellationToken);
        if (topUpResponseStatus == false)
        {
            return TopUpProcessStatus.ServiceProviderNotAvaliable;
        }

        //audit the transaction into application db.
        await transactionService.AddTransaction(userId, amount, phoneNumber, TopUpCharges);

        return TopUpProcessStatus.Compelete;
    }
}
