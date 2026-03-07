using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.InterviewRounds.Commands.UpdateInterviewRound
{
    public class UpdateInterviewRoundCommandHandler : IRequestHandler<UpdateInterviewRoundCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateInterviewRoundCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(
        UpdateInterviewRoundCommand request,
        CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var round = await _context.InterviewRounds
                .FirstOrDefaultAsync(
                    x => x.Id == request.Id && x.JobApplication.UserId == userId,
                    cancellationToken);

            if (round is null)
                throw new NotFoundException(nameof(InterviewRound), request.Id);

            round.Type = request.RoundType;
            round.Status = request.Status;
            round.ScheduledAt = request.ScheduledAt;
            round.InterviewerName = request.InterviewerName;
            round.Notes = request.Notes;
            round.Feedback = request.Feedback;
            round.DurationMinutes = request.DurationMinutes;
            round.MeetingLink = request.MeetingLink;
            round.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return round.Id;
        }
    }
}
