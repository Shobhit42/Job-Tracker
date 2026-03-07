using JobTracker.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.JobApplications.Commands.UpdateApplicationStatus
{
    public record UpdateApplicationStatusCommand : IRequest<Guid>
    {
        public required Guid Id { get; init; }
        public required ApplicationStatus Status { get; init; }
    }
}
