using Hangfire;
using Hangfire.PostgreSql;
using JobTracker.Application.Interfaces;
using JobTracker.Infrastructure.Identity;
using JobTracker.Infrastructure.Jobs;
using JobTracker.Infrastructure.Persistence;
using JobTracker.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
                ));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<AppDbContext>());

            services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<AppDbContext>();

            services.AddHttpContextAccessor();

            var jwtKey = configuration["JwtSettings:Secret"]
                ?? throw new InvalidOperationException("JWT Key is not configured.");
            var jwtIssuer = configuration["JwtSettings:Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer is not configured.");
            var jwtAudience = configuration["JwtSettings:Audience"]
                ?? throw new InvalidOperationException("JWT Audience is not configured.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, 
                    ValidateAudience = true,      
                    ValidateLifetime = true,      
                    ValidateIssuerSigningKey = true, 
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAuthService, AuthService>();
            var redisConnectionString = configuration["Redis:ConnectionString"];

            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(redisConnectionString!));
            services.AddScoped<ICacheService, RedisService>();

            services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(options =>
            {
                options.UseNpgsqlConnection(
                    configuration.GetConnectionString("DefaultConnection")!
                );
            }));
            services.AddHangfireServer();
            services.AddScoped<AutoGhostingJob>();
            services.AddScoped<InterviewReminderJob>();
            services.AddScoped<FollowUpNudgeJob>();
            services.AddScoped<RefreshTokenCleanupJob>();
            return services;
        }
    }
}
