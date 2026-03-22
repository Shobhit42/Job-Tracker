using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.JobApplications.Commands.AddJobApplication
{
    public class AddJobApplicationCommandValidator : AbstractValidator<AddJobApplicationCommand>
    {
        public AddJobApplicationCommandValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Company anem is required")
                .MaximumLength(100).WithMessage("Company name must not exceed 100 characters.");

            RuleFor(x => x.JobTitle)
            .NotEmpty().WithMessage("Job title is required.")
            .MaximumLength(100).WithMessage("Job title must not exceed 100 characters.");

            RuleFor(x => x.JobUrl)
                .MaximumLength(500).WithMessage("Job URL must not exceed 500 characters.")
                .When(x => x.JobUrl != null);

            RuleFor(x => x.SalaryOffered)
                .GreaterThan(0).WithMessage("Salary must be greater than 0.")
                .When(x => x.SalaryOffered.HasValue);

            RuleFor(x => x.AppliedDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Applied date cannot be in the future.");
        }
    }
}
