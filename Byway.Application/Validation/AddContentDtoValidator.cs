using Byway.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.Validation
{
    public class AddContentDtoValidator: AbstractValidator<AddContentDto>
    {

        public AddContentDtoValidator()
        {
            RuleFor(x => x.Name)
           .NotEmpty().WithMessage("Content name is required.")
           .MaximumLength(100).WithMessage("Content name cannot exceed 100 characters.");

            RuleFor(x => x.LecturesNumber)
                .GreaterThan(0).WithMessage("Lectures number must be positive.");

            RuleFor(x => x.TimeInMinutes)
                .GreaterThan(0).WithMessage("Time in minutes must be positive.");
        }
    }
}
