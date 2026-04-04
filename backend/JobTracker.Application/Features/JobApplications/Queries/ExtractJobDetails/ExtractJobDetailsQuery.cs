using JobTracker.Application.Features.JobApplications.DTOs;
using MediatR;

namespace JobTracker.Application.Features.JobApplications.Queries.ExtractJobDetails;

public record ExtractJobDetailsQuery(string JobUrl) : IRequest<ExtractedJobDetailsDto>;