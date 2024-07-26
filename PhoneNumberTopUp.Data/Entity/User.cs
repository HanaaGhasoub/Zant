namespace PhoneNumberTopUp.Data.Entity;

public class User
{
    public Guid Id { get; }
    public required string Name { get; init; }
    public required bool Verified { get; init; }
}
