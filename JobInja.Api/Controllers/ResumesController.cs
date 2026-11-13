using System.Security.Claims;
using JobInja.Api.Constants;
using JobInja.Api.Data;
using JobInja.Api.DTOs.Resumes;
using JobInja.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobInja.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ResumesController(
    ApplicationDbContext context,
    IWebHostEnvironment environment) : ControllerBase
{
    private const long MaxFileSizeBytes = 2 * 1024 * 1024;

    private string ResumesPhysicalFolder =>
        Path.Combine(environment.ContentRootPath, "PrivateFiles", "Resumes");

    [HttpPost]
    [Authorize(Roles = RoleNames.JobSeeker)]
    public async Task<ActionResult> Create([FromForm] CreateResumeRequestDto request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var extension = Path.GetExtension(request.File.FileName).ToLowerInvariant();

        if (extension != ".pdf")
        {
            return BadRequest(AppMessages.InvalidFileType);
        }

        if (request.File.Length > MaxFileSizeBytes)
        {
            return BadRequest(AppMessages.FileTooLarge);
        }

        Directory.CreateDirectory(ResumesPhysicalFolder);

        var storedFileName = $"{Guid.NewGuid()}{extension}";
        var physicalPath = Path.Combine(ResumesPhysicalFolder, storedFileName);

        await using (var stream = new FileStream(physicalPath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream);
        }

        var resume = new Resume
        {
            UserId = userId,
            Title = request.Title.Trim(),
            FileName = Path.GetFileName(request.File.FileName),
            FilePath = storedFileName
        };

        context.Resumes.Add(resume);
        await context.SaveChangesAsync();

        return Created($"api/resumes/{resume.Id}", new { Id = resume.Id });
    }

    [HttpGet("mine")]
    [Authorize(Roles = RoleNames.JobSeeker)]
    public async Task<ActionResult> GetMine()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var resumes = await context.Resumes
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.DateCreated)
            .Select(x => new ResumeResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                FileName = x.FileName,
                DateCreated = x.DateCreated
            })
            .ToListAsync();

        return Ok(resumes);
    }

    [HttpGet("{id}/file")]
    [Authorize]
    public async Task<ActionResult> DownloadFile(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var resume = await context.Resumes
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (resume is null)
        {
            return NotFound(AppMessages.NotFound);
        }

        var isOwner = resume.UserId == userId;

        var isEmployerOfApplicant = await context.JobApplications
            .AnyAsync(x => x.ResumeId == id && x.JobPost.Company.OwnerId == userId);

        if (!isOwner && !isEmployerOfApplicant)
        {
            return NotFound(AppMessages.ResumeNotFound);
        }

        var physicalPath = Path.Combine(ResumesPhysicalFolder, Path.GetFileName(resume.FilePath));

        if (!System.IO.File.Exists(physicalPath))
        {
            return NotFound(AppMessages.NotFound);
        }

        return PhysicalFile(physicalPath, "application/pdf", resume.FileName);
    }
}