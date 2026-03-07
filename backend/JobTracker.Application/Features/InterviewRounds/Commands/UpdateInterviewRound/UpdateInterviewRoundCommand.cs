using JobTracker.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.InterviewRounds.Commands.UpdateInterviewRound
{
    public record UpdateInterviewRoundCommand : IRequest<Guid>
    {
        public required Guid Id { get; init; } 
        public required RoundType RoundType { get; init; }
        public required RoundStatus Status { get; init; }
        public DateTime? ScheduledAt { get; init; }
        public string? InterviewerName { get; init; }
        public string? Notes { get; init; }
        public string? Feedback { get; init; }
        public int? DurationMinutes { get; init; }
        public string? MeetingLink { get; init; }
    }
}
