using BookingApi.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class BookingItemsController : ControllerBase
{
    private readonly BookingDBContext _context;

    public BookingItemsController(BookingDBContext context)
    {
        _context = context;
    }

    // 📅 Thêm mới booking item (đặt phòng)
    [HttpPost]
    public IActionResult AddBookingItem([FromBody] BookingItem item)
    {
        if (item.StartAt >= item.EndAt)
            return BadRequest(new { message = "⛔ Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc!" });

        var conflict = _context.BookingItems.Any(b =>
            b.ResourceId == item.ResourceId &&
            (
                (item.StartAt >= b.StartAt && item.StartAt < b.EndAt) ||
                (item.EndAt > b.StartAt && item.EndAt <= b.EndAt) ||
                (item.StartAt <= b.StartAt && item.EndAt >= b.EndAt)
            )
        );

        if (conflict)
            return BadRequest(new { message = "❌ Phòng đã có người đặt trong thời gian này." });

        _context.BookingItems.Add(item);
        _context.SaveChanges();

        return Ok(new { message = "✅ Đặt phòng thành công!" });
    }
}
