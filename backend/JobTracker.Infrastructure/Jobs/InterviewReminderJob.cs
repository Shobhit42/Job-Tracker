using JobTracker.Domain.Enums;
using JobTracker.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Jobs;

public class InterviewReminderJob
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<InterviewReminderJob> _logger;

    public InterviewReminderJob(IApplicationDbContext context, ILogger<InterviewReminderJob> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        var now = DateTime.UtcNow;

        var candidates = await _context.InterviewRounds
            .Include(r => r.JobApplication)
            .Where(r => r.Status == RoundStatus.Scheduled
                     && r.ScheduledAt.HasValue
                     && !r.ReminderSent                           
                     && r.ScheduledAt.Value >= now.AddMinutes(45)  
                     && r.ScheduledAt.Value <= now.AddMinutes(75)) 
            .ToListAsync();

        if (!candidates.Any())
        {
            _logger.LogInformation("InterviewReminderJob: No reminders to send.");
            return;
        }

        foreach (var round in candidates)
        {
            _logger.LogInformation(
                "InterviewReminderJob: 1h reminder — {RoundType} at {Company}, scheduled {ScheduledAt} UTC",
                round.Type,
                round.JobApplication.CompanyName,
                round.ScheduledAt);

            round.ReminderSent = true;
        }

        await _context.SaveChangesAsync(CancellationToken.None);

        _logger.LogInformation(
            "InterviewReminderJob: Sent {Count} reminder(s).", candidates.Count);
    }
}