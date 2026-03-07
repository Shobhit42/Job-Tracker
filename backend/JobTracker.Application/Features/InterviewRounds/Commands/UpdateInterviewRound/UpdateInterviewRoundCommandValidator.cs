using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.InterviewRounds.Commands.UpdateInterviewRound
{
    public class UpdateInterviewRoundCommandValidator : AbstractValidator<UpdateInterviewRoundCommand>
    {
        public UpdateInterviewRoundCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.RoundType).IsInEnum();
            RuleFor(x => x.Status).IsInEnum();
        }
    }
}
