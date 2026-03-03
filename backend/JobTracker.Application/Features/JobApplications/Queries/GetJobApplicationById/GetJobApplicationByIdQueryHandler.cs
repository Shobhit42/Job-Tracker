using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Application.Features.JobApplications.Queries.GetJobApplicationById
{
    public class GetJobApplicationByIdQueryHandler : IRequestHandler<GetJobApplicationByIdQuery, GetJobApplicationByIdDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetJobApplicationByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }
        public async Task<GetJobApplicationByIdDto> Handle(GetJobApplicationByIdQuery request, CancellationToken cancellationToken)
        {
            var query = _context.JobApplications.Where(a => a.Id == request.Id && a.UserId == _currentUserService.UserId);
            var result = await query.Select(x => new GetJobApplicationByIdDto
            {
                Id = x.Id,
                CompanyName = x.CompanyName,
                JobTitle = x.JobTitle,
                JobUrl = x.JobUrl,
                Description = x.JobDescription,
                Status = x.Status,
                Platform = x.Platform,
                SalaryOffered = x.SalaryOffered,
                AppliedDate = x.AppliedDate,
                CreatedAt = x.CreatedAt,
                Skills = x.JobApplicationSkills.Select(s => s.Skill.Name).ToList(),
                Tags = x.JobApplicationTags.Select(t => t.Tag.Name).ToList(),

                InterviewRoundDtos = x.InterviewRounds.Select(r => new InterviewRoundDto
                {
                    Id = r.Id,
                    RoundType = r.Type,
                    Status = r.Status,
                    ScheduledAt = r.ScheduledAt,
                    Notes = r.Notes
                }).ToList(),
            }).FirstOrDefaultAsync(cancellationToken);

            if (result is null)
                throw new NotFoundException(nameof(JobApplication), request.Id);

            return result;
        }
    }
}
