namespace PhoneNumberTopUp.Domain;

public class ServiceOptions
{
    public required string GetBalanceUrl { get; set; }

    public required string CreateDebitUrl { get; set; }
}
