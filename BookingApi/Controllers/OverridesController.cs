using BookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class OverridesController : ControllerBase
{
    private readonly BookingDBContext _context;

    public OverridesController(BookingDBContext context)
    {
        _context = context;
    }

    
    // 🔹 GET: /api/overrides?resourceId=1
    [HttpGet]

    public async Task<IActionResult> GetOverrides([FromQuery] int resourceId)
    {
        var list = await _context.AvailabilityOverrides
            .Include(o => o.Resource)
            .Where(o => o.ResourceId == resourceId)
            .OrderByDescending(o => o.StartAt)
            .Select(o => new
            {
                o.OverrideId,
                o.ResourceId,
                Name = o.Resource.Name, // ✅ lấy tên phòng
                o.StartAt,
                o.EndAt,
                o.IsAvailable,
                o.Note
            })
            .ToListAsync();

        return Ok(list);
    }


    // 🔹 POST: /api/overrides
    [HttpPost]
    public async Task<IActionResult> CreateOverride([FromBody] AvailabilityOverride request)
    {
        if (request.EndAt <= request.StartAt)
            return BadRequest(new { message = "Thời gian không hợp lệ." });
        request.Resource = null;
        request.IsAvailable = false;
        // ✅ Kiểm tra conflict với booking hiện có
        var conflict = await (from bki in _context.BookingItems
                              join bk in _context.Bookings on bki.BookingId equals bk.BookingId
                               where bki.ResourceId == request.ResourceId 
                               && bk.StatusId !=5
                                 && request.StartAt < bki.EndAt
          && request.EndAt > bki.StartAt select bki
        ).AnyAsync();

        if (conflict)
            return Conflict(new { message = "Phòng đang có booking trong thời gian này." });

        _context.AvailabilityOverrides.Add(request);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Đã khóa phòng thành công." });
    }

    // 🔹 DELETE: /api/overrides/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOverride(int id)
    {
        var item = await _context.AvailabilityOverrides.FindAsync(id);
        if (item == null)
            return NotFound(new { message = "Không tìm thấy khóa phòng." });

        _context.AvailabilityOverrides.Remove(item);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Đã bỏ khóa phòng." });
    }
}
