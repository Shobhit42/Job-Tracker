using JobTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.JobApplications.Commands.AddJobApplication
{
    public class AddJobApplicationDto
    {
        public required string CompanyName { get; init; }
        public required string JobTitle { get; init; }
        public string? JobUrl { get; init; }
        public string? JobDescription { get; init; }
        public string? Location { get; init; }
        public decimal? SalaryOffered { get; init; }
        public string? Notes { get; init; }
        public DateTime? OfferDeadline { get; init; }
        public Platform Platform { get; init; } = Platform.Other;
        public DateTime AppliedDate { get; init; } = DateTime.UtcNow;
    }
}
