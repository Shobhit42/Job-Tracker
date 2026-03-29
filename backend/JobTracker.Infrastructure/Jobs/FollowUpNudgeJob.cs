using JobTracker.Domain.Enums;
using JobTracker.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Jobs;

public class FollowUpNudgeJob
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<FollowUpNudgeJob> _logger;

    public FollowUpNudgeJob(IApplicationDbContext context, ILogger<FollowUpNudgeJob> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        var windowStart = DateTime.UtcNow.AddDays(-3).AddHours(-12);
        var windowEnd = DateTime.UtcNow.AddDays(-3).AddHours(12);

        var applicationsToNudge = await _context.JobApplications
            .Where(j => j.Status == ApplicationStatus.Applied
                     && !j.FollowUpSent                   
                     && j.AppliedDate >= windowStart
                     && j.AppliedDate <= windowEnd) 
            .ToListAsync();

        if (!applicationsToNudge.Any())
        {
            _logger.LogInformation("FollowUpNudgeJob: No follow-up nudges needed.");
            return;
        }

        foreach (var application in applicationsToNudge)
        {
            _logger.LogInformation(
                "FollowUpNudgeJob: Nudge for {JobTitle} at {Company}, applied {AppliedDate} UTC",
                application.JobTitle, 
                application.CompanyName,
                application.AppliedDate);

            application.FollowUpSent = true;
        }

        await _context.SaveChangesAsync(CancellationToken.None);

        _logger.LogInformation(
            "FollowUpNudgeJob: Sent {Count} nudge(s).", applicationsToNudge.Count);
    }
}