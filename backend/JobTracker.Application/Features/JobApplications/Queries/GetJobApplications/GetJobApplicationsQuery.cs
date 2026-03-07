using JobTracker.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.JobApplications.Queries.GetJobApplications
{
    public class GetJobApplicationsQuery : IRequest<List<GetJobApplicationDto>>
    {
        public ApplicationStatus? Status { get; init; }   
        public Platform? Platform { get; init; } 
        public string? Search { get; init; }
        public DateTime? DateFrom { get; init; }
        public DateTime? DateTo { get; init; }
    }
}
