using CustomerApi.Addresses.Models;

namespace CustomerApi.Addresses.Validators;

public class CreateAddressRequestValidator : AbstractValidator<CreateAddressRequest>
{
    public CreateAddressRequestValidator()
    {
        RuleFor(x => x.AddressName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.AddressText).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Phone).NotEmpty().MaximumLength(20);
    }
}
