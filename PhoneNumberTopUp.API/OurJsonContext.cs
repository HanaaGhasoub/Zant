using PhoneNumberTopUp.Data.Entity;
using System.Text.Json.Serialization;
using static PhoneNumberTopUp.API.Controllers.BeneficiariesController;
using static PhoneNumberTopUp.Controllers.TopUpController;

namespace PhoneNumberTopUp.API;

[JsonSerializable(typeof(TopUpOption))]
[JsonSerializable(typeof(BeneficiaryRecord))]
[JsonSerializable(typeof(TopUpRecord))]
public partial class OurJsonContext : JsonSerializerContext
{
}
