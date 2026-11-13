using System.Security.Claims;
using JobInja.Api.Constants;
using JobInja.Api.Data;
using JobInja.Api.DTOs.Companies;
using JobInja.Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobInja.Api.Controllers;

[Authorize(Roles = RoleNames.Employer)]
[Route("api/[controller]")]
[ApiController]
public class CompaniesController(ApplicationDbContext context) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create(CreateCompanyRequestDto request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var provinceExists = await context.Provinces
            .AnyAsync(x => x.Id == request.ProvinceId);

        if (!provinceExists)
        {
            return BadRequest(AppMessages.InvalidProvince);
        }

        var company = new Company
        {
            Name = request.Name.Trim(),
            Description = request.Description.Trim(),
            ProvinceId = request.ProvinceId,
            OwnerId = userId
        };

        context.Companies.Add(company);
        await context.SaveChangesAsync();

        return Created($"api/companies/{company.Id}", new { Id = company.Id });
    }

    [HttpGet("mine")]
    public async Task<ActionResult> GetMine()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var companies = await context.Companies
            .AsNoTracking()
            .Where(x => x.OwnerId == userId)
            .Select(x => new CompanyResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ProvinceId = x.ProvinceId,
                ProvinceName = x.Province.Name
            })
            .ToListAsync();

        return Ok(companies);
    }
}