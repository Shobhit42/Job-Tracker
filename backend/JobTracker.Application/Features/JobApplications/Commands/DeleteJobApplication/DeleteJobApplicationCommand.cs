using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.JobApplications.Commands.DeleteJobApplication
{
    public record DeleteJobApplicationCommand : IRequest<Guid>
    {
        public required Guid Id { get; init; }
    }
}
