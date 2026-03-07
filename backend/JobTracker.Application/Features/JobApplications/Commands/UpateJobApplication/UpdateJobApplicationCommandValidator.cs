using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.JobApplications.Commands.UpateJobApplication
{
    internal class UpdateJobApplicationCommandValidator : AbstractValidator<UpdateJobApplicationCommand>
    {
        public UpdateJobApplicationCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.CompanyName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.JobTitle).NotEmpty().MaximumLength(200);
            RuleFor(x => x.JobUrl).MaximumLength(1000).When(x => x.JobUrl != null);
        }
    }
}
