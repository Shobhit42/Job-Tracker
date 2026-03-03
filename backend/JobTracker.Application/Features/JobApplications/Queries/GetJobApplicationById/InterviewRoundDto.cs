using JobTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.JobApplications.Queries.GetJobApplicationById
{
    public class InterviewRoundDto
    {
        public Guid Id { get; init; }
        public RoundType RoundType { get; init; }
        public RoundStatus Status { get; init; }
        public DateTime? ScheduledAt { get; init; }
        public string? Notes { get; init; }
    }
}
