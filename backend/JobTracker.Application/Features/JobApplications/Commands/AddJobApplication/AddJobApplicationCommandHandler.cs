using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;
using MediatR;

namespace JobTracker.Application.Features.JobApplications.Commands.AddJobApplication
{
    public class AddJobApplicationCommandHandler : IRequestHandler<AddJobApplicationCommand, Guid>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICacheService _cache;

        public AddJobApplicationCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, ICacheService cache)
        {
            _context = context;
            _currentUserService = currentUserService;
            _cache = cache;
        }

        public async Task<Guid> Handle(AddJobApplicationCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId!;
            var application = new JobApplication
            {
                UserId = userId,
                CompanyName = request.CompanyName,
                JobTitle = request.JobTitle,
                JobUrl = request.JobUrl,
                JobDescription = request.JobDescription,
                Location = request.Location,
                SalaryOffered = request.SalaryOffered,
                Notes = request.Notes,
                OfferDeadline = request.OfferDeadline,
                Platform = request.Platform,
                Status = ApplicationStatus.Applied,
                AppliedDate = request.AppliedDate,
                AiScraped = false,
                FollowUpSent = false
            };

            _context.JobApplications.Add(application);
            await _context.SaveChangesAsync(cancellationToken);
            await _cache.RemoveAsync($"job-applications:user:{_currentUserService.UserId}");
            return application.Id;
        }
    }
}
