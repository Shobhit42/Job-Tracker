using JobTracker.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.InterviewRounds.Commands.AddInterviewRounds
{
    public class AddInterviewRoundDto
    {
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
