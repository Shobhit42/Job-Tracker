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
    public class JobApplicationTagConfiguration : IEntityTypeConfiguration<JobApplicationTag>
    {
        public void Configure(EntityTypeBuilder<JobApplicationTag> builder)
        {
            // Same idea — JobApplicationId + TagId together = unique row
            builder.HasKey(x => new { x.JobApplicationId, x.TagId });

            builder.HasOne(x => x.JobApplication)
                .WithMany(x => x.JobApplicationTags)
                .HasForeignKey(x => x.JobApplicationId);

            builder.HasOne(x => x.Tag)
                .WithMany(x => x.JobApplicationTags)
                .HasForeignKey(x => x.TagId);
        }
    }
}
