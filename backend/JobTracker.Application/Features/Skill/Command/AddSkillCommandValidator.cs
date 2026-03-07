using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.Skill.Command
{
    public class AddSkillCommandValidator : AbstractValidator<AddSkillCommand>
    {
        public AddSkillCommandValidator()
        {
            RuleFor(x => x.JobApplicationId).NotEmpty();
            RuleFor(x => x.SkillName).NotEmpty().MaximumLength(100);
        }
    }
}
