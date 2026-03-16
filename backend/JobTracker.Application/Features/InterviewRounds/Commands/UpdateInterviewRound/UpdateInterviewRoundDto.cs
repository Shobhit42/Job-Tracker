using JobTracker.Domain.Enums;

namespace JobTracker.Application.Features.InterviewRounds.Commands.UpdateInterviewRound;

public class UpdateInterviewRoundDto
{
    public required RoundType RoundType { get; init; }
    public required RoundStatus Status { get; init; }
    public DateTime? ScheduledAt { get; init; }
    public string? InterviewerName { get; init; }
    public string? Notes { get; init; }
    public string? Feedback { get; init; }
    public int? DurationMinutes { get; init; }
    public string? MeetingLink { get; init; }
}