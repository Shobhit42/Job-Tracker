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
    public class InterviewRoundConfiguration : IEntityTypeConfiguration<InterviewRound>
    {
        public void Configure(EntityTypeBuilder<InterviewRound> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Type)
                .HasConversion<int>();

            builder.Property(x => x.Status)
                .HasConversion<int>();

            builder.Property(x => x.Notes)
                .HasMaxLength(2000);

            builder.Property(x => x.InterviewerName)
                .HasMaxLength(200);
        }
    }
}
