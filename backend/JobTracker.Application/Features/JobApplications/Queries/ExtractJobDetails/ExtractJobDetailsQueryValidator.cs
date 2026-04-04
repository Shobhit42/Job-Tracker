using FluentValidation;

namespace JobTracker.Application.Features.JobApplications.Queries.ExtractJobDetails;

public class ExtractJobDetailsQueryValidator : AbstractValidator<ExtractJobDetailsQuery>
{
    public ExtractJobDetailsQueryValidator()
    {
        RuleFor(x => x.JobUrl)
            .NotEmpty().WithMessage("Job URL is required.")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Must be a valid URL.");
    }
}