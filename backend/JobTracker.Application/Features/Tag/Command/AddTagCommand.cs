using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.Tag.Command
{
    public record AddTagCommand : IRequest<Guid>
    {
        public required Guid JobApplicationId { get; init; }
        public required string TagName { get; init; } 
    }
}
