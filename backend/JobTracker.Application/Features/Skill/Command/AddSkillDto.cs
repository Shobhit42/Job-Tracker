namespace JobTracker.Application.Features.Skill.Command;

public class AddSkillDto
{
    public required string SkillName { get; init; }
    public string? Category { get; init; }
}