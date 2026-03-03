using JobTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.JobApplications.Queries.GetJobApplicationById
{
    public class GetJobApplicationByIdDto
    {
        public Guid Id { get; init; }
        public required string CompanyName { get; init; }
        public required string JobTitle { get; init; }
        public string? JobUrl { get; init; }
        public string? Description { get; init; }
        public ApplicationStatus Status { get; init; }
        public Platform Platform { get; init; }
        public decimal? SalaryOffered { get; init; }
        public DateTime AppliedDate { get; init; }
        public DateTime CreatedAt { get; init; }
        public List<string> Skills { get; init; } = new();
        public List<string> Tags { get; init; } = new();

        public List<InterviewRoundDto> InterviewRoundDtos { get; init; } = new List<InterviewRoundDto>();
    }
}
