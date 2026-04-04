namespace JobTracker.Application.Features.JobApplications.DTOs;

public class ExtractedJobDetailsDto
{
    public string CompanyName { get; set; } = string.Empty;
    public string JobTitle { get; set; } = string.Empty;
    public string? Location { get; set; }
    public string? JobDescription { get; set; }
    public List<string> Skills { get; set; } = new();
    public int Platform { get; set; } = 8;
}