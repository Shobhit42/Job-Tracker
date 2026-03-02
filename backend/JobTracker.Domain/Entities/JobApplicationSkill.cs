namespace JobTracker.Domain.Entities
{
    public class JobApplicationSkill
    {
        public Guid JobApplicationId { get; set; }
        public Guid SkillId { get; set; }
        public JobApplication JobApplication { get; set; } = null!;
        public Skill Skill { get; set; } = null!;
    }
}