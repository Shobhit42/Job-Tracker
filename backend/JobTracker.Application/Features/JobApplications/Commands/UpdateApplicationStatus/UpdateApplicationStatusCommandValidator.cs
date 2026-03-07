using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.JobApplications.Commands.UpdateApplicationStatus
{
    public class UpdateApplicationStatusCommandValidator : AbstractValidator<UpdateApplicationStatusCommand>
    {
        public UpdateApplicationStatusCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Status).IsInEnum();
        }
    }
}
