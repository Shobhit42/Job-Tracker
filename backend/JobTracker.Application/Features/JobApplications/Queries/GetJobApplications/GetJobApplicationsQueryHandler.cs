using JobTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Application.Features.JobApplications.Queries.GetJobApplications
{
    public class GetJobApplicationsQueryHandler : IRequestHandler<GetJobApplicationsQuery, List<GetJobApplicationDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        public GetJobApplicationsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService) 
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<List<GetJobApplicationDto>> Handle(GetJobApplicationsQuery request, CancellationToken cancellationToken)
        {
            var jobApplications = _context.JobApplications.Where(application => application.UserId == _currentUserService.UserId);

            if (request.Status.HasValue)
                jobApplications = jobApplications.Where(application => application.Status == request.Status.Value);
            if (request.Platform.HasValue)
                jobApplications = jobApplications.Where(application => application.Platform == request.Platform.Value);
            if (!string.IsNullOrWhiteSpace(request.Search))
                jobApplications = jobApplications.Where(application => application.JobTitle.Contains(request.Search) || application.CompanyName.Contains(request.Search));
            if (request.DateFrom.HasValue)
                jobApplications = jobApplications.Where(application => application.AppliedDate >= request.DateFrom);
            if (request.DateTo.HasValue)
                jobApplications = jobApplications.Where(application => application.AppliedDate <= request.DateTo);

            return await jobApplications
                .OrderByDescending(x => x.AppliedDate)
                .Select(x => new GetJobApplicationDto 
                {                                       
                    Id = x.Id,
                    CompanyName = x.CompanyName,
                    JobTitle = x.JobTitle,
                    JobUrl = x.JobUrl,
                    Status = x.Status,
                    Platform = x.Platform,
                    SalaryOffered = x.SalaryOffered,
                    AppliedDate = x.AppliedDate,
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync(cancellationToken);
        }
    }
}
