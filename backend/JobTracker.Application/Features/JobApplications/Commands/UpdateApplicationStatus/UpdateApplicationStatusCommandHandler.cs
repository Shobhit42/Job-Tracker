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

namespace JobTracker.Application.Features.JobApplications.Commands.UpdateApplicationStatus
{
    public class UpdateApplicationStatusCommandHandler
    : IRequestHandler<UpdateApplicationStatusCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICacheService _cache;

        public UpdateApplicationStatusCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService,
            ICacheService cache)
        {
            _context = context;
            _currentUserService = currentUserService;
            _cache = cache;
        }

        public async Task<Guid> Handle(
            UpdateApplicationStatusCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var application = await _context.JobApplications.FirstOrDefaultAsync(
                    x => x.Id == request.Id && x.UserId == userId,
                    cancellationToken);

            if (application is null)
                throw new NotFoundException(nameof(JobApplication), request.Id);

            application.Status = request.Status;
            application.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);
            await _cache.RemoveAsync($"job-applications:user:{_currentUserService.UserId}");

            return application.Id;
        }
    }
}
