using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Infrastructure.Persistence.Configurations
{
    public class JobApplicationSkillConfiguration : IEntityTypeConfiguration<JobApplicationSkill>
    {
        public void Configure(EntityTypeBuilder<JobApplicationSkill> builder)
        {
            // Composite PK — the combination of both columns is the unique identifier
            // JobApplicationId + SkillId together = one unique row
            // You can't add the same skill to the same application twice
            builder.HasKey(x => new { x.JobApplicationId, x.SkillId });

            // JobApplicationSkill belongs to one JobApplication
            builder.HasOne(x => x.JobApplication)
                .WithMany(x => x.JobApplicationSkills)
                .HasForeignKey(x => x.JobApplicationId);

            // JobApplicationSkill belongs to one Skill
            builder.HasOne(x => x.Skill)
                .WithMany(x => x.JobApplicationSkills)
                .HasForeignKey(x => x.SkillId);
        }
    }
}
