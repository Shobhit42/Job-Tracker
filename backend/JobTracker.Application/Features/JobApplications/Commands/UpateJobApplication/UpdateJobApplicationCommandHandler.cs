using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.JobApplications.Commands.UpateJobApplication
{
    internal class UpdateJobApplicationCommandHandler : IRequestHandler<UpdateJobApplicationCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateJobApplicationCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<Guid> Handle(UpdateJobApplicationCommand request, CancellationToken cancellationToken)
        {
            var application = await _context.JobApplications.FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == _currentUserService.UserId, cancellationToken);

            if (application == null)
                throw new Domain.Exceptions.NotFoundException(nameof(JobApplication), request.Id);

            application.CompanyName = request.CompanyName;
            application.JobTitle = request.JobTitle;
            application.JobUrl = request.JobUrl;
            application.JobDescription = request.JobDescription;
            application.Platform = request.Platform;
            application.Notes = request.Notes;
            application.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return application.Id;
        }
    }
}
