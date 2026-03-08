using JobTracker.Application.Interfaces;
using JobTracker.Domain.Entities;
using JobTracker.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>, IApplicationDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<JobApplication> JobApplications => Set<JobApplication>();
        public DbSet<InterviewRound> InterviewRounds => Set<InterviewRound>();
        public DbSet<Skill> Skills => Set<Skill>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<JobApplicationSkill> JobApplicationSkills => Set<JobApplicationSkill>();
        public DbSet<JobApplicationTag> JobApplicationTags => Set<JobApplicationTag>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // MUST call base — applies all Identity table configs first
            base.OnModelCreating(builder);

            // Apply all IEntityTypeConfiguration<T> classes from this assembly automatically
            // This keeps AppDbContext clean — each entity's config lives in its own file
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }


    }
}
