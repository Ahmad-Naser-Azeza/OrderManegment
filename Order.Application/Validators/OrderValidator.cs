
using FluentValidation;
using Order.Domain.Models;
using SharedKernel;

namespace CoreOps.FleetManagment.Application.Validators;

public sealed class OrderValidator : AbstractValidator<OrdersModel>
{
    public OrderValidator(Dispatcher dispatcher)
    {
        
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("Please Fill Customer Name");

        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("Please Fill Product Name");

        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Please Fill Quantity")
            .GreaterThanOrEqualTo(0).WithMessage("Quantity must be greater than or equal to 0");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Please Fill Price")
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0");

        RuleFor(x => (int)x.Status)
            .InclusiveBetween(0, 2).WithMessage("Status must be between 0 and 2");

    }
}

