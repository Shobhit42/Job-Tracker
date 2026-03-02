namespace JobTracker.Domain.Entities
{
    public class Tag
    {
        public required string Name { get; set; }
        public required string UserId { get; set; }
        public ICollection<JobApplicationTag> JobApplicationTags { get; set; } = null!;
    }
}