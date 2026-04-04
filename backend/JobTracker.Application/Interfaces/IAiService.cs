using JobTracker.Application.Features.JobApplications.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Interfaces
{
    public interface IAiService
    {
        Task<ExtractedJobDetailsDto?> ExtractJobDetailsAsync(string pageUrl);
    }
}
