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

namespace JobTracker.Application.Features.Skill.Command
{
    public class AddSkillCommandHandler : IRequestHandler<AddSkillCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public AddSkillCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(AddSkillCommand request, CancellationToken cancellationToken)
        {
            var applicationExists = await _context.JobApplications.AnyAsync(x => x.Id == request.JobApplicationId && x.UserId == _currentUserService.UserId);

            if(!applicationExists)
                throw new NotFoundException(nameof(JobApplication), request.JobApplicationId);

            var existingSkill = await _context.Skills.FirstOrDefaultAsync(x => x.Name == request.SkillName && x.UserId == _currentUserService.UserId, cancellationToken);

            if(existingSkill is null)
            {
                existingSkill = new JobTracker.Domain.Entities.Skill
                {
                    Name = request.SkillName,
                    Category = request.Category,
                    UserId = _currentUserService.UserId,
                };
                _context.Skills.Add(existingSkill);
            }

            var alreadyLinked = await _context.JobApplicationSkills.AnyAsync(x => x.JobApplicationId == request.JobApplicationId && x.Skill.Name == request.SkillName);

            if(!alreadyLinked)
            {
                var jobApplicationSkill = new JobApplicationSkill
                {
                    JobApplicationId = request.JobApplicationId,
                    SkillId = existingSkill.Id,
                };
                _context.JobApplicationSkills.Add(jobApplicationSkill);
            }

            await _context.SaveChangesAsync();

            return existingSkill.Id;
        }
    }
}
