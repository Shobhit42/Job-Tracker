using JobTracker.Domain.Common;

namespace JobTracker.Domain.Entities
{
    public class Skill : BaseEntity
    {
        public required string Name { get; set; }
        public string? Category { get; set; }
        public required string UserId { get; set; }
        public ICollection<JobApplicationSkill> JobApplicationSkills { get; set; } = null!;
    }
}