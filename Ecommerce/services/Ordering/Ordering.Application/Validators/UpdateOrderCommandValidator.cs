

using FluentValidation;
using Ordering.Application.Commands;

namespace Ordering.Application.Validators;

public class UpdateOrderCommandValidator:AbstractValidator<UpdateOrderCommand>
{
    public UpdateOrderCommandValidator()
    {
        RuleFor(o => o.Id)
           .NotEmpty()
           .NotNull()
           .WithMessage("{Id} is Required")
           .GreaterThan(0)
           .WithMessage("{Id} can not be -ve");

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
