using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.InterviewRounds.Commands.AddInterviewRounds
{
    public class AddInterviewRoundCommandValidator : AbstractValidator<AddInterviewRoundCommand>
    {
        public AddInterviewRoundCommandValidator()
        {
            RuleFor(x => x.JobApplicationId).NotEmpty();
            RuleFor(x => x.Type).IsInEnum();
            RuleFor(x => x.Status).IsInEnum();
            RuleFor(x => x.ScheduledAt).NotEmpty();
            RuleFor(x => x.RoundNumber).GreaterThan(0);
        }
    }
}
