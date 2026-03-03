using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Features.JobApplications.Queries.GetJobApplicationById
{
    public class GetJobApplicationByIdQuery : IRequest<GetJobApplicationByIdDto>
    {
        public Guid Id { get; init; }
    }
}
