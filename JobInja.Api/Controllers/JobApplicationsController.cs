using System.Security.Claims;
using JobInja.Api.Constants;
using JobInja.Api.Data;
using JobInja.Api.DTOs.JobApplications;
using JobInja.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobInja.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobApplicationsController(ApplicationDbContext context) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = RoleNames.JobSeeker)]
    public async Task<ActionResult> Create(CreateJobApplicationRequestDto request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var jobPostActive = await context.JobPosts
            .AnyAsync(x => x.Id == request.JobPostId && x.IsActive);

        if (!jobPostActive)
        {
            return BadRequest(AppMessages.JobPostNotFound);
        }

        var resumeOwnedByUser = await context.Resumes
            .AnyAsync(x => x.Id == request.ResumeId && x.UserId == userId);

        if (!resumeOwnedByUser)
        {
            return BadRequest(AppMessages.ResumeNotFound);
        }

        var alreadyApplied = await context.JobApplications
            .AnyAsync(x => x.JobPostId == request.JobPostId && x.UserId == userId);

        if (alreadyApplied)
        {
            return BadRequest(AppMessages.AlreadyApplied);
        }

        var application = new JobApplication
        {
            JobPostId = request.JobPostId,
            ResumeId = request.ResumeId,
            UserId = userId,
            Status = ApplicationStatus.Submitted
        };

        context.JobApplications.Add(application);
        await context.SaveChangesAsync();

        return Created($"api/jobapplications/{application.Id}", new { Id = application.Id });
    }

    [HttpGet("mine")]
    [Authorize(Roles = RoleNames.JobSeeker)]
    public async Task<ActionResult> GetMine()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var applications = await context.JobApplications
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.DateCreated)
            .Select(x => new JobApplicationResponseDto
            {
                Id = x.Id,
                JobPostId = x.JobPostId,
                JobPostTitle = x.JobPost.Title,
                CompanyName = x.JobPost.Company.Name,
                Status = x.Status,
                DateCreated = x.DateCreated
            })
            .ToListAsync();

        return Ok(applications);
    }

    [HttpGet("by-post/{jobPostId}")]
    [Authorize(Roles = RoleNames.Employer)]
    public async Task<ActionResult> GetByJobPost(int jobPostId)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var jobPostOwnedByUser = await context.JobPosts
            .AnyAsync(x => x.Id == jobPostId && x.Company.OwnerId == userId);

        if (!jobPostOwnedByUser)
        {
            return BadRequest(AppMessages.JobPostNotFound);
        }

        var applicants = await context.JobApplications
            .AsNoTracking()
            .Where(x => x.JobPostId == jobPostId)
            .OrderByDescending(x => x.DateCreated)
            .Select(x => new ApplicantResponseDto
            {
                Id = x.Id,
                FullName = x.User.FirstName + " " + x.User.LastName,
                Mobile = x.User.Mobile,
                ResumeTitle = x.Resume.Title,
                ResumeId = x.ResumeId,
                Status = x.Status,
                DateCreated = x.DateCreated
            })
            .ToListAsync();

        return Ok(applicants);
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = RoleNames.Employer)]
    public async Task<ActionResult> UpdateStatus(int id, UpdateApplicationStatusRequestDto request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var application = await context.JobApplications
            .Include(x => x.JobPost)
            .ThenInclude(x => x.Company)
            .FirstOrDefaultAsync(x => x.Id == id && x.JobPost.Company.OwnerId == userId);

        if (application is null)
        {
            return BadRequest(AppMessages.JobApplicationNotFound);
        }

        application.Status = request.Status;
        await context.SaveChangesAsync();

        return NoContent();
    }
}