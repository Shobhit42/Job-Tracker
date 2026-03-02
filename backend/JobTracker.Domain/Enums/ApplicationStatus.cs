using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Domain.Enums
{
    public enum ApplicationStatus
    {
        Applied = 1,
        Shortlisted = 2,
        InProgress = 3,
        Offer = 4,
        Rejected = 5,
        Ghosted = 6,
        IDeclined = 7
    }
}
