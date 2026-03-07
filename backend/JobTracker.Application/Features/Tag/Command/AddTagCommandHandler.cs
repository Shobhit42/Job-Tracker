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

namespace JobTracker.Application.Features.Tag.Command
{
    public class AddTagCommandHandler : IRequestHandler<AddTagCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public AddTagCommandHandler(
            IApplicationDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(AddTagCommand request, CancellationToken cancellationToken)
        {
            var applicationExists = await _context.JobApplications
            .AnyAsync( x => x.Id == request.JobApplicationId && x.UserId == _currentUserService.UserId, cancellationToken);

            if (!applicationExists)
                throw new NotFoundException(nameof(JobApplication), request.JobApplicationId);

            var tag = await _context.Tags
            .FirstOrDefaultAsync(
                x => x.Name == request.TagName && x.UserId == _currentUserService.UserId,
                cancellationToken);

            if (tag is null)
            {
                tag = new JobTracker.Domain.Entities.Tag
                {
                    Name = request.TagName,
                    UserId = _currentUserService.UserId,
                };
                _context.Tags.Add(tag);
            }

            var alreadyLinked = await _context.JobApplicationTags
            .AnyAsync(
                x => x.JobApplicationId == request.JobApplicationId && x.TagId == tag.Id,
                cancellationToken);

            if (!alreadyLinked)
            {
                var jobApplicationTag = new JobApplicationTag
                {
                    JobApplicationId = request.JobApplicationId,
                    TagId = tag.Id,
                };

                _context.JobApplicationTags.Add(jobApplicationTag);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return tag.Id;
        }
    }
}
