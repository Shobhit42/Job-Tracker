using JobTracker.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace JobTracker.Infrastructure.Jobs;

public class RefreshTokenCleanupJob
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<RefreshTokenCleanupJob> _logger;

    public RefreshTokenCleanupJob(IApplicationDbContext context, ILogger<RefreshTokenCleanupJob> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        var now = DateTime.UtcNow;
        var deadTokens = await _context.RefreshTokens
            .Where(t => t.IsActive)
            .ToListAsync();

        if (!deadTokens.Any())
        {
            _logger.LogInformation("RefreshTokenCleanupJob: No expired or revoked tokens to delete.");
            return;
        }

        _context.RefreshTokens.RemoveRange(deadTokens);
        await _context.SaveChangesAsync(CancellationToken.None);

        _logger.LogInformation(
            "RefreshTokenCleanupJob: Deleted {Count} expired/revoked refresh tokens.",
            deadTokens.Count);
    }
}