using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.Tag.Command
{
    public class AddTagCommandValidator : AbstractValidator<AddTagCommand>
    {
        public AddTagCommandValidator()
        {
            RuleFor(x => x.JobApplicationId).NotEmpty();
            RuleFor(x => x.TagName).NotEmpty().MaximumLength(50);
        }
    }
}
