using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.JobApplications.Commands.AddJobApplication
{
    public class AddJobApplicationDto
    {
        public Guid Id { get; init; }
        public string CompanyName { get; init; } = string.Empty;
        public string JobTitle { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public DateTime AppliedDate { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
