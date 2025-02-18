using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Json.SystemTextJson.JsonConverters;

namespace ABPCourse.Demo1.Patient
{
    public class PatientValidator: AbstractValidator<Patient>
    {
        public PatientValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            //RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Date of birth is required");
            RuleFor(x => x.ContactNumber)
                .NotEmpty().WithMessage("Contact number is required")
                .Must(l => l != null && l.Length == 11).WithMessage("The phone number must be exactly 11 digits.");

        }
    }
}
