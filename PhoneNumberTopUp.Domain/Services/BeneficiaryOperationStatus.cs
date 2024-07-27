namespace PhoneNumberTopUp.Domain.Services;

public enum BeneficiaryOperationStatus
{
    BeneficiaryAdded,
    BeneficiaryRemoved,
    BeneficiaryNotFound,
    ServerError,
    ValidationError
}
