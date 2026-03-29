using JobTracker.Application.Interfaces;
using JobTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Infrastructure.Jobs
{
    public class AutoGhostingJob
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<AutoGhostingJob> _logger;
        public AutoGhostingJob(IApplicationDbContext context, ILogger<AutoGhostingJob> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task ExecuteAsync()
        {
            var cutoff = DateTime.UtcNow.AddDays(-15);
            var staleApplications = await _context.JobApplications
            .Where(j => j.Status == ApplicationStatus.Applied
                     && j.UpdatedAt < cutoff)
            .ToListAsync();

            if (!staleApplications.Any())
            {
                _logger.LogInformation("AutoGhostingJob: No stale applications found.");
                return;
            }

            foreach (var application in staleApplications)
            {
                application.Status = ApplicationStatus.Ghosted;
                application.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync(CancellationToken.None);
            _logger.LogInformation("AutoGhostingJob: Marked {Count} applications as Ghosted.",staleApplications.Count);
        }
    }
}
