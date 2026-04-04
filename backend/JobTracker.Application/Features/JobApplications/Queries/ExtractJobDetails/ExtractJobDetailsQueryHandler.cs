using JobTracker.Application.Features.JobApplications.DTOs;
using JobTracker.Application.Interfaces;
using JobTracker.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace JobTracker.Application.Features.JobApplications.Queries.ExtractJobDetails;

public class ExtractJobDetailsQueryHandler
    : IRequestHandler<ExtractJobDetailsQuery, ExtractedJobDetailsDto>
{
    private readonly IAiService _aiService;
    private readonly ILogger<ExtractJobDetailsQueryHandler> _logger;

    public ExtractJobDetailsQueryHandler(
        IAiService aiService,
        ILogger<ExtractJobDetailsQueryHandler> logger)
    {
        _aiService = aiService;
        _logger = logger;
    }

    public async Task<ExtractedJobDetailsDto> Handle(
        ExtractJobDetailsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Extracting job details from: {Url}", request.JobUrl);

        var extracted = await _aiService.ExtractJobDetailsAsync(request.JobUrl);

        if (extracted == null)
        {
            _logger.LogWarning("AI returned null for URL: {Url}", request.JobUrl);
            return new ExtractedJobDetailsDto();
        }

        extracted.Platform = InferPlatform(request.JobUrl);
        return extracted;
    }

    private static int InferPlatform(string url)
    {
        var lower = url.ToLowerInvariant();
        return lower switch
        {
            _ when lower.Contains("linkedin.com") => (int)Platform.LinkedIn,
            _ when lower.Contains("naukri.com") => (int)Platform.Naukri,
            _ when lower.Contains("internshala.com") => (int)Platform.Internshala,
            _ when lower.Contains("indeed.com") => (int)Platform.Indeed,
            _ when lower.Contains("instahyre.com") => (int)Platform.Instahyre,
            _ => (int)Platform.Other
        };
    }
}