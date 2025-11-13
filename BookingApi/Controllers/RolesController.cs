using BookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly BookingDBContext _context;
    public RolesController(BookingDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await _context.Roles
            .Select(r => new { r.RoleId, r.RoleName })
            .ToListAsync();
        return Ok(roles);
    }
}
