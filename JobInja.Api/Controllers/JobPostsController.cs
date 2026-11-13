using System.Security.Claims;
using JobInja.Api.Constants;
using JobInja.Api.Data;
using JobInja.Api.DTOs.JobPosts;
using JobInja.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobInja.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobPostsController(ApplicationDbContext context) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = RoleNames.Employer)]
    public async Task<ActionResult> Create(CreateJobPostRequestDto request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var companyOwnedByUser = await context.Companies
            .AnyAsync(x => x.Id == request.CompanyId && x.OwnerId == userId);

        if (!companyOwnedByUser)
        {
            return BadRequest(AppMessages.CompanyNotFound);
        }

        var provinceExists = await context.Provinces
            .AnyAsync(x => x.Id == request.ProvinceId);

        if (!provinceExists)
        {
            return BadRequest(AppMessages.InvalidProvince);
        }

        var jobPost = new JobPost
        {
            CompanyId = request.CompanyId,
            ProvinceId = request.ProvinceId,
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            SalaryText = request.SalaryText?.Trim(),
            IsActive = true
        };

        context.JobPosts.Add(jobPost);
        await context.SaveChangesAsync();

        return Created($"api/jobposts/{jobPost.Id}", new { Id = jobPost.Id });
    }

    [HttpGet]
    public async Task<ActionResult> Search([FromQuery] JobPostSearchRequestDto request)
    {
        var query = context.JobPosts
            .AsNoTracking()
            .Where(x => x.IsActive);

        if (!string.IsNullOrWhiteSpace(request.Keyword))
        {
            var keyword = request.Keyword.Trim();
            query = query.Where(x => x.Title.Contains(keyword) || x.Description.Contains(keyword));
        }

        if (request.ProvinceId.HasValue)
        {
            query = query.Where(x => x.ProvinceId == request.ProvinceId.Value);
        }

        var items = await query
            .OrderByDescending(x => x.DateCreated)
            .Select(x => new JobPostResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                SalaryText = x.SalaryText,
                CompanyName = x.Company.Name,
                ProvinceName = x.Province.Name,
                DateCreated = x.DateCreated
            })
            .ToListAsync();

        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        var jobPost = await context.JobPosts
            .AsNoTracking()
            .Where(x => x.Id == id && x.IsActive)
            .Select(x => new JobPostResponseDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                SalaryText = x.SalaryText,
                CompanyName = x.Company.Name,
                ProvinceName = x.Province.Name,
                DateCreated = x.DateCreated
            })
            .FirstOrDefaultAsync();

        if (jobPost is null)
        {
            return NotFound(AppMessages.NotFound);
        }

        return Ok(jobPost);
    }

    [HttpPut("{id}/deactivate")]
    [Authorize(Roles = RoleNames.Employer)]
    public async Task<ActionResult> Deactivate(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var jobPost = await context.JobPosts
            .Include(x => x.Company)
            .FirstOrDefaultAsync(x => x.Id == id && x.Company.OwnerId == userId);

        if (jobPost is null)
        {
            return BadRequest(AppMessages.JobPostNotFound);
        }

        jobPost.IsActive = false;
        await context.SaveChangesAsync();

        return NoContent();
    }
}