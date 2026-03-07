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

        public UpdateApplicationStatusCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
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

            return application.Id;
        }
    }
}
