using JobTracker.Domain.Enums;

namespace JobTracker.Application.Features.JobApplications.Commands.UpdateApplicationStatus;

public class UpdateApplicationStatusDto
{
    public required ApplicationStatus Status { get; init; }
}