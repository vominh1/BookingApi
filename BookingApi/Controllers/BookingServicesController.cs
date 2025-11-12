using BookingApi.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class BookingServicesController : ControllerBase
{
    private readonly BookingDBContext _context;
    public BookingServicesController(BookingDBContext context)
    {
        _context = context;
    }

    // Thêm dịch vụ vào Booking
    [HttpPost]
    public IActionResult AddService([FromBody] BookingServiceDto dto)
    {
        if (dto == null || dto.Quantity <= 0)
            return BadRequest(new { message = "Dữ liệu không hợp lệ" });

        var booking = _context.Bookings.FirstOrDefault(b => b.BookingCode == dto.BookingCode);
        var service = _context.Services.Find(dto.ServiceId);

        if (booking == null || service == null)
            return NotFound(new { message = "Không tìm thấy booking hoặc dịch vụ" });

        var newItem = new BookingService
        {
            BookingId = booking.BookingId,
            ServiceId = dto.ServiceId,
            Quantity = dto.Quantity,
            UnitPrice = service.Price
        };

        _context.BookingServices.Add(newItem);
        _context.SaveChanges(); // Lưu trước để tính tổng có dữ liệu mới

        
        var totalItems = _context.BookingItems
            .Where(x => x.BookingId == booking.BookingId)
            .Select(x => (decimal?)x.Price * x.Quantity)
            .Sum() ?? 0;

        var totalServices = _context.BookingServices
            .Where(x => x.BookingId == booking.BookingId)
            .Select(x => (decimal?)x.UnitPrice * x.Quantity)
            .Sum() ?? 0;

        booking.TotalAmount = totalItems + totalServices;
        _context.SaveChanges();

        return Ok(new { message = "✅ Đã thêm dịch vụ vào booking", booking.TotalAmount });
    }

}
