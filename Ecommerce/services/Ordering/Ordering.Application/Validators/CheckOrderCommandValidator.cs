using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators;

public class CheckOrderCommandValidator:AbstractValidator<CheckoutOrderCommand>
{
    public CheckOrderCommandValidator()
    {
        RuleFor(o => o.UserName)
            .NotEmpty()
            .WithMessage("{UserName} is Required")
            .NotNull()
            .MaximumLength(70)
            .WithMessage("{UserName} must not exceed 70 characters ");

        RuleFor(o => o.TotalPrice)
           .NotEmpty()
           .WithMessage("{TotalPrice} is Required")
           .NotNull()
           .GreaterThan(-1)
           .WithMessage("{TotalPrice} should not be -ve");

        RuleFor(o => o.EmailAddress)
       .NotEmpty()
       .WithMessage("{EmailAddress} is Required");

        RuleFor(o => o.FirstName)
        .NotEmpty()
        .NotNull()
        .WithMessage("{FirstName} is Required");



        RuleFor(o => o.LastName)
        .NotEmpty()
        .NotNull()
        .WithMessage("{LastName} is Required");


    }

}
