using Byway.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.Validation
{
    public class CreateInstructorDtoValidator: AbstractValidator<CreateInstructorDto>
    {
        public CreateInstructorDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

            RuleFor(x => x.JobTitle)
                .IsInEnum().WithMessage("A valid job title is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.Image)
                .NotEmpty().WithMessage("A profile image is required.");

            When(x => x.Image != null, () =>
            {
               
                RuleFor(x => x.Image.Length)
                    .LessThanOrEqualTo(5 * 1024 * 1024) 
                    .WithMessage("Image size must not exceed 5 MB.");

               
                RuleFor(x => x.Image.ContentType)
                    .Must(type => type.Equals("image/jpeg") || type.Equals("image/png"))
                    .WithMessage("Invalid image format. Only JPG and PNG files are allowed.");
            });
        }


    }
}
