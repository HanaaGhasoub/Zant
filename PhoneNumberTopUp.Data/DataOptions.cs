namespace PhoneNumberTopUp.Data;

public class DataOptions
{
    public int MaxBeneficiaryCountPerUser { get; set; } = 5;

    public int MaxBeneficiaryNicknameLength { get; set; } = 20;
}
