using BookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class ServicesController : ControllerBase
{
    private readonly BookingDBContext _context;
    public ServicesController(BookingDBContext context)
    {
        _context = context;
    }

    // 🧾 Lấy danh sách dịch vụ
    [HttpGet]
    public IActionResult GetAllServices()
    {
        var services = _context.Services
            .Where(s => s.IsActive)
            .Select(s => new
            {
                s.ServiceId,
                s.Name,
                s.Description,
                s.Price
            }).ToList();

        return Ok(services);
    }
}
