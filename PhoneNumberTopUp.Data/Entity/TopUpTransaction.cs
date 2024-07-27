namespace PhoneNumberTopUp.Data.Entity;

public class TopUpTransaction
{
    public Guid TransactionId { get; set; }

    public Guid UserId { get; set; }

    public int PhoneNumber { get; set; }

    public int Amount { get; set; }

    public int Charges { get; set; }

    public DateTime TransactionDateTime { get; set; } = DateTime.Now;
}
