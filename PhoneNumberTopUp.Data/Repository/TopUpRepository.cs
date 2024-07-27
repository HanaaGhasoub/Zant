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

    public async Task<TopUpTransaction?> AddTransaction(Guid userId, int amount, int phoneNumber, int topUpCharges)
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

            var result = await topUpDb.SaveChangesAsync();

            return result == 1 ? transaction : null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, string.Empty);
            return null;
        }
    }

    public async Task<List<TopUpOption>> GetTopUpOptions()
    {
        return await topUpDb.TopUpOptions.ToListAsync();
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
