using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhoneNumberTopUp.Data.Entity;

namespace PhoneNumberTopUp.Data.Repository;

public class TopUpRepository : ITopUpRepository
{
    private readonly TopUpDb topUpDb;
    private readonly ILogger<TopUpRepository> logger;

    public TopUpRepository(TopUpDb topUpDb, ILogger<TopUpRepository> logger)
    {
        this.topUpDb = topUpDb;
        this.logger = logger;
    }

    public async Task<int> AddTransaction(Guid userId, int amount, int phoneNumber, int topUpCharges)
    {
        try
        {
            var transaction = new TopUpTransaction
            {
                Amount = amount,
                PhoneNumber = phoneNumber,
                UserId = userId,
                Charges = topUpCharges
            };

            topUpDb.TopUpTransactions.Add(transaction);

            return await topUpDb.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Empty);
            return 0;
        }
    }

    public List<TopUpOption> GetTopUpOptions()
    {
        return [
            new () { DisplayName = "AED 5", Value = 5 },
            new () { DisplayName = "AED 10", Value = 10 },
            new () { DisplayName = "AED 20", Value = 20 },
            new () { DisplayName = "AED 30", Value = 30 },
            new () { DisplayName = "AED 50", Value = 50 },
            new () { DisplayName = "AED 75", Value = 75 },
            new () { DisplayName = "AED 100", Value = 100 } ];
    }

    public async Task<List<TopUpTransaction>> GetTransactionsInThisMonth(Guid userId)
    {
        var currentMonth = DateTime.Now.Month;

        var transactionsInTheCurrentMonth = await (from transaction in topUpDb.TopUpTransactions
                                                   where transaction.UserId == userId &&
                                                   transaction.TransactionDateTime.Month == currentMonth
                                                   select transaction)
                                                   .ToListAsync();

        return transactionsInTheCurrentMonth;
    }
}
