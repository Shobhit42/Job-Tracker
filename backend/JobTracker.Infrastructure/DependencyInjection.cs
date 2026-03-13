using JobTracker.Application.Interfaces;
using JobTracker.Infrastructure.Identity;
using JobTracker.Infrastructure.Persistence;
using JobTracker.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobTracker.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
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

            var jwtKey = configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("JWT Key is not configured.");
            var jwtIssuer = configuration["Jwt:Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer is not configured.");
            var jwtAudience = configuration["Jwt:Audience"]
                ?? throw new InvalidOperationException("JWT Audience is not configured.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,           // Token must come from our server
                    ValidateAudience = true,         // Token must be meant for our app
                    ValidateLifetime = true,         // Token must not be expired
                    ValidateIssuerSigningKey = true, // Token must be signed with our secret key
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;
        }
    }
}
