using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byway.Application.Validation
{
    public class UpdateCourseDtoValidation: FluentValidation.AbstractValidator<Byway.Application.DTOs.UpdateCourseDto>
    {
        public UpdateCourseDtoValidation() {

            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Course name is required.")
            .MaximumLength(200).WithMessage("Course name cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Course description is required.")
                .MaximumLength(2000).WithMessage("Course description cannot exceed 2000 characters.");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be a non-negative value.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.Level)
                .IsInEnum().WithMessage("A valid course level is required.");

            RuleFor(x => x.TotalHours)
                .GreaterThan(0).WithMessage("Total hours must be a positive value.");

            RuleFor(x => x.Certification)
                .NotEmpty().WithMessage("Certification information is required.")
                .MaximumLength(300).WithMessage("Certification information cannot exceed 300 characters.");

            RuleFor(x => x.InstructorId)
                .GreaterThan(0).WithMessage("A valid instructor ID is required.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("A valid category ID is required.");
            RuleFor(x => x.Contents)
           .NotEmpty().WithMessage("At least one content item is required.");

            RuleForEach(x => x.Contents).SetValidator(new AddContentDtoValidator());
            When(x => x.ImageFile != null, () =>
            {
                RuleFor(x => x.ImageFile)
                    .Must(file => file.Length <= 5 * 1024 * 1024) // 5 MB
                    .WithMessage("Image size must not exceed 5 MB.")
                    .Must(file => new[] { "image/jpeg", "image/png", "image/gif" }.Contains(file.ContentType))
                    .WithMessage("Invalid image format. Supported formats are JPG, PNG, GIF.");
            });


        }
    }
}
