using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.Skill.Command
{
    public record AddSkillCommand : IRequest<Guid>
    {
        public required Guid JobApplicationId { get; init; }
        public required string SkillName { get; init; }
        public string? Category { get; init; }
    }
}
