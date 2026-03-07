using JobTracker.Domain.Common;

namespace JobTracker.Domain.Entities
{
    public class Tag : BaseEntity
    {
        public required string Name { get; set; }
        public required string UserId { get; set; }
        public ICollection<JobApplicationTag> JobApplicationTags { get; set; } = null!;
    }
}