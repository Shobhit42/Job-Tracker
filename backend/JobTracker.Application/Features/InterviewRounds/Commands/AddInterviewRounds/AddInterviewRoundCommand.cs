using JobTracker.Domain.Enums;
using MediatR;

namespace JobTracker.Application.Features.InterviewRounds.Commands.AddInterviewRounds
{
    public record AddInterviewRoundCommand : IRequest<Guid>
    {
        public required Guid JobApplicationId { get; init; }
        public required int RoundNumber { get; init; }
        public required RoundType Type { get; init; }
        public RoundStatus Status { get; init; } = RoundStatus.Scheduled;
        public DateTime? ScheduledAt { get; init; }
        public string? InterviewerName { get; init; }
        public string? Notes { get; init; }
        public string? Feedback { get; init; }
        public int? DurationMinutes { get; init; }
        public string? MeetingLink { get; init; }
    }
}
