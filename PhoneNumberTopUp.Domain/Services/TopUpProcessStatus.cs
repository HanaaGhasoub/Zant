namespace PhoneNumberTopUp.Domain.Services;

public enum TopUpProcessStatus
{
    Compelete,    
    UserNotFound,
    TransactionsLimitReached,
    InsufficientBalance,
    ServiceProviderNotAvaliable
}
