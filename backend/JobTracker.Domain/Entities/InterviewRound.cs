using JobTracker.Domain.Common;
using JobTracker.Domain.Enums;

namespace JobTracker.Domain.Entities;

public class InterviewRound : BaseEntity
{
    public Guid JobApplicationId { get; set; }
    public int RoundNumber { get; set; }
    public RoundType Type { get; set; }
    public RoundStatus Status { get; set; } = RoundStatus.Scheduled;
    public DateTime? ScheduledAt { get; set; }
    public string? InterviewerName { get; set; }
    public string? Notes { get; set; }
    public string? Feedback { get; set; }
    public int? DurationMinutes { get; set; }
    public string? MeetingLink { get; set; }
    public bool ReminderSent { get; set; } = false;
    public JobApplication JobApplication { get; set; } = null!;
}