using JobTracker.Domain.Enums;

namespace JobTracker.Application.Features.JobApplications.Commands.UpateJobApplication;

public class UpdateJobApplicationDto
{
    public required string CompanyName { get; init; }
    public required string JobTitle { get; init; }
    public string? JobUrl { get; init; }
    public string? JobDescription { get; init; }
    public Platform Platform { get; init; }
    public string? Notes { get; init; }
}