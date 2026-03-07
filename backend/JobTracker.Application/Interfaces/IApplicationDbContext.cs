using JobTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<JobApplication> JobApplications { get; }
        DbSet<InterviewRound> InterviewRounds { get; }
        DbSet<Skill> Skills { get; }
        DbSet<Tag> Tags { get; }
        DbSet<JobApplicationSkill> JobApplicationSkills { get; }
        DbSet<JobApplicationTag> JobApplicationTags { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
