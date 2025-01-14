using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace ABPCourse.Demo1.Products
{
    public class CreateAndUpdateProductValidator:AbstractValidator<CreateAndUpdateProductDto>
    {
        public CreateAndUpdateProductValidator()
        {
            RuleFor(x => x.NameAr)
                .NotEmpty()
                .WithMessage("Arabic name is required.")
                .MaximumLength(200)
                .WithMessage("Arabic name must not exceed 200 characters.");

            RuleFor(x => x.NameEn)
                .NotEmpty()
                .WithMessage("English name is required.")
                .MaximumLength(200)
                .WithMessage("English name must not exceed 200 characters.");

            RuleFor(x => x.DescriptionAr)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(1000)
                .WithMessage("Arabic description must not exceed 1000 characters.");

            RuleFor(x => x.DescriptionEn)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(1000)
                .WithMessage("English description must not exceed 1000 characters.");

            RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Category ID must be greater than 0.");
        }
        
    }
}
