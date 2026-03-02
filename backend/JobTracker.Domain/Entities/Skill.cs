namespace JobTracker.Domain.Entities
{
    public class Skill
    {
        public required string Name { get; set; }
        public string? Category { get; set; }
        public ICollection<JobApplicationSkill> JobApplicationSkills { get; set; } = null!;
    }
}