using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ABPCourse.Demo1.Payment
{
    public class PaymentValidator:AbstractValidator<payment>
    {
        public PaymentValidator()
        {

            RuleFor(p => p.TransactionId)
                .NotEmpty().WithMessage("Transaction ID is required.")
                .MaximumLength(50).WithMessage("Transaction ID cannot exceed 50 characters.");

            RuleFor(p => p.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");

            RuleFor(p => p.PaymentStatus)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => status == "Pending" || status == "Completed" || status == "Failed")
                .WithMessage("Status must be 'Pending', 'Completed', or 'Failed'.");

            RuleFor(p => p.PayerName)
                .NotEmpty().WithMessage("Payer name is required.")
                .MaximumLength(100).WithMessage("Payer name cannot exceed 100 characters.");
        }
    }
}
