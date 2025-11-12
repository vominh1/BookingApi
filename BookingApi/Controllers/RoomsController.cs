using BookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Data.SqlClient;



[Route("api/[controller]")]
[ApiController]
public class RoomsController : ControllerBase
{
    private readonly BookingDBContext _context;

    public RoomsController(BookingDBContext context)
    {
        _context = context;
    }



    // 🏠 Lấy danh sách phòng
    [HttpGet]
    public IActionResult GetAllRooms()
    {
        var rooms = _context.Resources.Select(r => new
        {
            r.ResourceId,
            r.Name,
            r.Description,
            r.Location,
            r.Price,
        }).ToList();

        return Ok(rooms);
    }
    [HttpGet("available")]
    public IActionResult CheckAvailable(int resourceId, DateTime start, DateTime end, decimal price)
    {
        if (start >= end)
        {
            return BadRequest(new { message = "⛔ Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc!" });
        }

        var conflict = _context.BookingItems.Any(b => b.ResourceId == resourceId && (start >= b.StartAt && start < b.EndAt || end > b.StartAt && end <= b.EndAt || start <= b.StartAt && end >= b.EndAt));
        return Ok(new { available = !conflict });
    }

    [HttpPost("book")]
    public IActionResult BookRoom([FromBody] BookingItemDto dto)
    {
        Console.WriteLine("📥 Nhận request POST /api/rooms/book");

        if (dto == null)
            return BadRequest(new { message = "Dữ liệu đặt phòng không hợp lệ!" });

        if (dto.StartAt >= dto.EndAt)
            return BadRequest(new { message = "⛔ Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc!" });

        // 🔍 Kiểm tra trùng phòng
        bool conflict = _context.BookingItems.Any(b =>
            b.ResourceId == dto.ResourceId &&
            (
                (dto.StartAt >= b.StartAt && dto.StartAt < b.EndAt) ||
                (dto.EndAt > b.StartAt && dto.EndAt <= b.EndAt) ||
                (dto.StartAt <= b.StartAt && dto.EndAt >= b.EndAt)
            ));

        if (conflict)
            return BadRequest(new { message = "❌ Phòng đã được đặt trong khoảng thời gian này!" });

        string bookingCode = "BK" + Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();



        try
        {
            var bookingId = _context.Database.SqlQuery<int>(
     $"EXEC CreateBooking @BookingCode = '{bookingCode}', @CustomerId = 1, @UserId = 1,@TotalAmount={dto.Price * dto.Quantity}, @Note = '{dto.Note ?? ""}'"
 ).AsEnumerable().FirstOrDefault();

            var newItem = new BookingItem
            {
                BookingId = bookingId,
                ResourceId = dto.ResourceId,
                StartAt = dto.StartAt,
                EndAt = dto.EndAt,
                Price = dto.Price,
                Quantity = dto.Quantity,
                Note = dto.Note
            };

            _context.BookingItems.Add(newItem);
            _context.SaveChanges();

            return Ok(new
            {
                message = "✅ Đặt phòng thành công!",
                bookingId,
                bookingCode
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"💥 Lỗi khi đặt phòng: {ex.Message}" });
        }

    }
    [HttpGet("bycode/{bookingCode}/items")]
    public IActionResult GetBookingItemsByCode(string bookingCode)
    {
        try
        {
            // 🔹 Lấy danh sách phòng thuộc booking có BookingCode tương ứng
            var items = _context.BookingItems
                .Include(b => b.Resource) // Lấy thêm thông tin phòng
                .Include(b => b.Booking)  // Để lọc theo BookingCode
                .Where(b => b.Booking.BookingCode == bookingCode)
                .Select(b => new
                {
                    b.BookingItemId,
                    b.ResourceId,
                    ResourceName = b.Resource.Name,
                    b.StartAt,
                    b.EndAt,
                    b.Price,
                    b.Quantity,
                    b.Note
                })
                .ToList();

            if (!items.Any())
                return NotFound(new { message = "❌ Không tìm thấy phòng trong booking này!" });

            return Ok(items);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });



        }
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetResources()
    {
        try
        {
            var list = await _context.Resources
                .Select(r => new
                {
                    r.ResourceId,
                    r.Name
                })
                .OrderBy(r => r.Name)
                .ToListAsync();

            return Ok(list);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Lỗi server: " + ex.Message });
        }
    }

}