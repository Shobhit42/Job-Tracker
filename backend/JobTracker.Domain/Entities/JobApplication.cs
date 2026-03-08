using JobTracker.Domain.Common;
using JobTracker.Domain.Enums;

namespace JobTracker.Domain.Entities;

public class JobApplication : BaseEntity
{
    public Guid UserId { get; set; }
    public string CompanyName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string? JobUrl { get; set; }
    public string? JobDescription { get; set; }
    public Platform Platform { get; set; } = Platform.Other;
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;
    public string? Location { get; set; }
    public decimal? SalaryOffered { get; set; }
    public DateTime AppliedDate { get; set; } = DateTime.UtcNow;
    public DateTime? OfferDeadline { get; set; }
    public string? Notes { get; set; }
    public bool AiScraped { get; set; } = false;
    public bool FollowUpSent { get; set; } = false;
    public ICollection<InterviewRound> InterviewRounds { get; set; } = new List<InterviewRound>();
    public ICollection<JobApplicationSkill> JobApplicationSkills { get; set; } = null!;
    public ICollection<JobApplicationTag> JobApplicationTags { get; set; } = null!;
}