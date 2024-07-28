namespace PhoneNumberTopUp.Data.Entity;

public class Beneficiary
{
    public int Id { get; }

    public required Guid UserId { get; init; }

    public required int PhoneNumber { get; init; }

    public required string Nickname { get; init; }

    public bool Deleted { get; set; }
}
