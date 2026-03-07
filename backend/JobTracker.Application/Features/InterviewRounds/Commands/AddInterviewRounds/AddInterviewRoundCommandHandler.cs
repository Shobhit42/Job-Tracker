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

namespace JobTracker.Application.Features.InterviewRounds.Commands.AddInterviewRounds
{
    public class AddInterviewRoundCommandHandler : IRequestHandler<AddInterviewRoundCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public AddInterviewRoundCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(AddInterviewRoundCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var applicationExists = await _context.JobApplications
                .AnyAsync(
                    x => x.Id == request.JobApplicationId && x.UserId == userId,
                    cancellationToken);

            if (!applicationExists)
                throw new NotFoundException(nameof(JobApplication), request.JobApplicationId);

            var round = new InterviewRound
            {
                JobApplicationId = request.JobApplicationId,
                RoundNumber = request.RoundNumber,
                Type = request.Type,
                Status = request.Status,
                ScheduledAt = request.ScheduledAt,
                InterviewerName = request.InterviewerName,
                Notes = request.Notes,
                Feedback = request.Feedback,
                DurationMinutes = request.DurationMinutes,
                MeetingLink = request.MeetingLink
            };

            _context.InterviewRounds.Add(round);
            await _context.SaveChangesAsync(cancellationToken);

            return round.Id;
        }
    }
}
