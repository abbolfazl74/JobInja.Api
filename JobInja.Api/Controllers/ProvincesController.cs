using JobInja.Api.Data;
using JobInja.Api.DTOs.Provinces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobInja.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProvincesController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var provinces = await context.Provinces
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => new ProvinceResponseDto
            {
                Id = x.Id,
                Name = x.Name
            })
            .ToListAsync();

        return Ok(provinces);
    }
}