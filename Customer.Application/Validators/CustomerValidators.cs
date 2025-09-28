using FluentValidation;
using Customer.Application.DTOs;

namespace Customer.Application.Validators;

public class CreateCustomerDtoValidator : AbstractValidator<CreateCustomerRequestDto>
{
    public CreateCustomerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address format")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters");

        RuleFor(x => x.Address)
            .SetValidator(new AddressDtoValidator() as IValidator<AddressDto?>)
            .When(x => x.Address != null);
    }
}

public class UpdateCustomerDtoValidator : AbstractValidator<UpdateCustomerRequestDto>
{
    public UpdateCustomerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email address format")
            .MaximumLength(100).WithMessage("Email must not exceed 100 characters");

        RuleFor(x => x.Address)
            .SetValidator(new AddressDtoValidator())
            .When(x => x.Address != null);
    }
}

public class AddressDtoValidator : AbstractValidator<AddressDto?>
{
    public AddressDtoValidator()
    {
        When(x => x != null, () =>
        {
            RuleFor(x => x!.Country)
                .NotEmpty().WithMessage("Country is required")
                .MaximumLength(100).WithMessage("Country must not exceed 100 characters");

            RuleFor(x => x!.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(100).WithMessage("City must not exceed 100 characters");

            RuleFor(x => x!.Street)
                .NotEmpty().WithMessage("Street is required")
                .MaximumLength(200).WithMessage("Street must not exceed 200 characters");

            RuleFor(x => x!.Number)
                .GreaterThan(0).WithMessage("Number must be greater than 0");
        });
    }
}