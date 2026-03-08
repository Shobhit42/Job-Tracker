using JobTracker.Domain.Entities;
using JobTracker.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobTracker.Infrastructure.Persistence.Configurations;

public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        // Primary key — EF Core can guess this from "Id" but explicit is better
        builder.HasKey(x => x.Id);

        // Column constraints
        builder.Property(x => x.CompanyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.JobTitle)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.JobUrl)
            .HasMaxLength(1000); // optional field, no IsRequired

        builder.Property(x => x.Location)
            .HasMaxLength(200);

        builder.Property(x => x.Notes)
            .HasMaxLength(2000);

        // Store enum as int in DB (default behavior, explicit for clarity)
        builder.Property(x => x.Status)
            .HasConversion<int>();

        builder.Property(x => x.Platform)
            .HasConversion<int>();

        // Relationships
        // One JobApplication has many InterviewRounds
        // If JobApplication is deleted → delete its InterviewRounds too (Cascade)
        builder.HasMany(x => x.InterviewRounds)
            .WithOne(x => x.JobApplication)
            .HasForeignKey(x => x.JobApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        // One JobApplication has many JobApplicationSkills (join table)
        builder.HasMany(x => x.JobApplicationSkills)
            .WithOne(x => x.JobApplication)
            .HasForeignKey(x => x.JobApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        // One JobApplication has many JobApplicationTags (join table)
        builder.HasMany(x => x.JobApplicationTags)
            .WithOne(x => x.JobApplication)
            .HasForeignKey(x => x.JobApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}