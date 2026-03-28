using JobTracker.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace JobTracker.Application.Features.JobApplications.Queries.GetJobApplications
{
    public class GetJobApplicationsQueryHandler : IRequestHandler<GetJobApplicationsQuery, List<GetJobApplicationDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICacheService _cache;
        private readonly ILogger<GetJobApplicationsQueryHandler> _logger;
        public GetJobApplicationsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, ICacheService cache,
        ILogger<GetJobApplicationsQueryHandler> logger) 
        {
            _context = context;
            _currentUserService = currentUserService;
            _cache = cache;
            _logger = logger;
        }

        public async Task<List<GetJobApplicationDto>> Handle(GetJobApplicationsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            var cacheKey = $"job-applications:user:{userId}";
            var cached = await _cache.GetAsync(cacheKey);
            if (cached is not null)
            {
                _logger.LogInformation("Cache HIT for {CacheKey}", cacheKey);
                return JsonSerializer.Deserialize<List<GetJobApplicationDto>>(cached)!;
            }

            _logger.LogInformation("Cache MISS for {CacheKey}. Fetching from DB.", cacheKey);

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

            var applications = await jobApplications
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

            await _cache.SetAsync(cacheKey, JsonSerializer.Serialize(applications), TimeSpan.FromMinutes(5));

            return applications;
        }
    }
}
