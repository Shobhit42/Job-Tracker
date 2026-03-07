using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.JobApplications.Commands.DeleteJobApplication
{
    public class DeleteJobApplicationCommandValidator : AbstractValidator<DeleteJobApplicationCommand>
    {
        public DeleteJobApplicationCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
