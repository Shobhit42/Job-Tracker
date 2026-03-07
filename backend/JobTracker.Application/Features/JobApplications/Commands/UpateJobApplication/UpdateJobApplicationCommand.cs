using JobTracker.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.JobApplications.Commands.UpateJobApplication
{
    internal class UpdateJobApplicationCommand : IRequest<Guid>
    {
        public required Guid Id { get; init; }
        public required string CompanyName { get; init; }
        public required string JobTitle { get; init; }
        public string? JobUrl { get; init; }
        public string? JobDescription { get; init; }
        public Platform Platform { get; init; }
        public string? Notes { get; init; }
    }
}
