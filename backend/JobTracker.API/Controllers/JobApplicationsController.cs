using JobTracker.Application.Features.InterviewRounds.Commands.AddInterviewRounds;
using JobTracker.Application.Features.InterviewRounds.Commands.UpdateInterviewRound;
using JobTracker.Application.Features.JobApplications.Commands.AddJobApplication;
using JobTracker.Application.Features.JobApplications.Commands.DeleteJobApplication;
using JobTracker.Application.Features.JobApplications.Commands.UpateJobApplication;
using JobTracker.Application.Features.JobApplications.Commands.UpdateApplicationStatus;
using JobTracker.Application.Features.JobApplications.Queries.GetJobApplicationById;
using JobTracker.Application.Features.JobApplications.Queries.GetJobApplications;
using JobTracker.Application.Features.Skill.Command;
using JobTracker.Application.Features.Tag.Command;
using JobTracker.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JobTracker.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/job-applications")]
    public class JobApplicationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JobApplicationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddJobApplication([FromBody] AddJobApplicationDto dto, CancellationToken cancellationToken)
        {
            var command = new AddJobApplicationCommand
            {
                CompanyName = dto.CompanyName,
                JobTitle = dto.JobTitle,
                JobUrl = dto.JobUrl,
                JobDescription = dto.JobDescription,
                Location = dto.Location,
                SalaryOffered = dto.SalaryOffered,
                Notes = dto.Notes,
                OfferDeadline = dto.OfferDeadline,
                Platform = dto.Platform,
                AppliedDate = dto.AppliedDate
            };

            var newId = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(Application.Features.JobApplications.Queries.GetJobApplicationById), new { id = newId }, new { id = newId });
        }

        [HttpGet]
        public async Task<IActionResult> GetJobApplications(
            [FromQuery] ApplicationStatus? status,
            [FromQuery] Platform? platform,
            [FromQuery] string? search,
            CancellationToken cancellationToken)
        {
            var query = new GetJobApplicationsQuery
            {
                Status = status,
                Platform = platform,
                Search = search
            };

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetJobApplicationById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var query = new GetJobApplicationByIdQuery { Id = id };

            var result = await _mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateJobApplication(
            Guid id,
            [FromBody] UpdateJobApplicationDto dto,
            CancellationToken cancellationToken)
        {
            var command = new UpdateJobApplicationCommand
            {
                Id = id,
                CompanyName = dto.CompanyName,
                JobTitle = dto.JobTitle,
                JobUrl = dto.JobUrl,
                JobDescription = dto.JobDescription,
                Platform = dto.Platform,
                Notes = dto.Notes
            };

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteJobApplication(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteJobApplicationCommand { Id = id };

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<IActionResult> UpdateApplicationStatus(
            [FromRoute] Guid id,
            [FromBody] UpdateApplicationStatusDto dto,
            CancellationToken cancellationToken)
        {
            var command = new UpdateApplicationStatusCommand
            {
                Id = id,
                Status = dto.Status
            };

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpPost("{id:guid}/interview-rounds")]
        public async Task<IActionResult> AddInterviewRound(
            Guid id,
            [FromBody] AddInterviewRoundDto dto,
            CancellationToken cancellationToken)
        {
            var command = new AddInterviewRoundCommand
            {
                JobApplicationId = id, 
                RoundNumber = dto.RoundNumber,
                Type = dto.Type,
                Status = dto.Status,
                ScheduledAt = dto.ScheduledAt,
                InterviewerName = dto.InterviewerName,
                Notes = dto.Notes,
                Feedback = dto.Feedback,
                DurationMinutes = dto.DurationMinutes,
                MeetingLink = dto.MeetingLink
            };

            var newId = await _mediator.Send(command, cancellationToken);

            // 201 Created — round was created, return its Id
            // No CreatedAtAction here because we don't have a GetInterviewRoundById endpoint
            return Created(string.Empty, new { id = newId });
        }


        [HttpPut("{id:guid}/interview-rounds/{roundId:guid}")]
        public async Task<IActionResult> UpdateInterviewRound(
            Guid id,
            Guid roundId,
            [FromBody] UpdateInterviewRoundDto dto,
            CancellationToken cancellationToken)
        {
            var command = new UpdateInterviewRoundCommand
            {
                Id = roundId,       // the specific round being updated
                RoundType = dto.RoundType,
                Status = dto.Status,
                ScheduledAt = dto.ScheduledAt,
                InterviewerName = dto.InterviewerName,
                Notes = dto.Notes,
                Feedback = dto.Feedback,
                DurationMinutes = dto.DurationMinutes,
                MeetingLink = dto.MeetingLink
            };

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpPost("{id:guid}/skills")]
        public async Task<IActionResult> AddSkill(
            Guid id,
            [FromBody] AddSkillDto dto,
            CancellationToken cancellationToken)
        {
            var command = new AddSkillCommand
            {
                JobApplicationId = id,
                SkillName = dto.SkillName,
                Category = dto.Category
            };

            var skillId = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, new { id = skillId });
        }

        [HttpPost("{id:guid}/tags")]
        public async Task<IActionResult> AddTag(
            Guid id,
            [FromBody] AddTagDto dto,
            CancellationToken cancellationToken)
        {
            var command = new AddTagCommand
            {
                JobApplicationId = id,
                TagName = dto.TagName
            };

            var tagId = await _mediator.Send(command, cancellationToken);

            return Created(string.Empty, new { id = tagId });
        }
    }
}
