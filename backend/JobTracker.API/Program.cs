using Hangfire;
using JobTracker.API.Middleware;
using JobTracker.Application;   
using JobTracker.Infrastructure;
using JobTracker.Infrastructure.Jobs;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);


builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
builder.Services.AddTransient<ExceptionHandlingMiddleware>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()   // TODO: restrict to specific origins in production
            .AllowAnyMethod()   // GET, POST, PUT, DELETE, OPTIONS (preflight)
            .AllowAnyHeader();  // Authorization, Content-Type, etc.
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "Smart Job Tracker API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter your JWT token. Example: eyJhbGci..."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[] { new Hangfire.Dashboard.LocalRequestsOnlyAuthorizationFilter() }
});

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Smart Job Tracker API v1");
        options.RoutePrefix = string.Empty;
    });
}
app.UseCors("AllowAll");
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

RecurringJob.AddOrUpdate<AutoGhostingJob>(
    recurringJobId: "auto-ghosting",          // Unique ID shown in the dashboard
    methodCall: job => job.ExecuteAsync(),     // Which method to call
    cronExpression: Cron.Daily(2)             // Every day at 2:00 AM UTC
);

RecurringJob.AddOrUpdate<InterviewReminderJob>(
    recurringJobId: "interview-reminders",
    methodCall: job => job.ExecuteAsync(),
    cronExpression: "*/15 * * * *"
);

RecurringJob.AddOrUpdate<FollowUpNudgeJob>(
    recurringJobId: "follow-up-nudge",
    methodCall: job => job.ExecuteAsync(),
    cronExpression: Cron.Daily(9)
);

RecurringJob.AddOrUpdate<RefreshTokenCleanupJob>(
    recurringJobId: "refresh-token-cleanup",
    methodCall: job => job.ExecuteAsync(),
    cronExpression: Cron.Daily(3)
);


app.Run();
