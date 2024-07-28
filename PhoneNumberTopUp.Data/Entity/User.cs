namespace PhoneNumberTopUp.Data.Entity;

public class User
{
    public Guid Id { get; set; }

    public required bool Verified { get; init; }

    public required int PhoneNumber { get; set; }
}
