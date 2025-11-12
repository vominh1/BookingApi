using BookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly BookingDBContext _context;

    public PaymentsController(BookingDBContext context)
    {
        _context = context;
    }
    // POST /api/payments
    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] PaymentDto request)
    {
        if (request.Amount <= 0)
            return BadRequest(new { message = "Số tiền thanh toán phải lớn hơn 0." });

        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.BookingCode == request.BookingCode);

        if (booking == null)
            return NotFound(new { message = "Không tìm thấy booking với mã này." });

        // kiểm tra nếu booking có rồi thì không cho thanh toán nửa 
        bool extstsPayment = await _context.Payments
            .AnyAsync(p => p.BookingId == booking.BookingId);
        if (extstsPayment)
        {
            return BadRequest(new { message = "Booking này đã được thanh toán." });
        }
            var payment = new Payment
        {
            BookingId = booking.BookingId,
            Amount = request.Amount,
            PaymentMethod = request.PaymentMethod,
            TransactionRef = request.TransactionRef,
            PaidAt = DateTime.Now
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();

        var totalPaid = await _context.Payments
            .Where(p => p.BookingId == booking.BookingId)
            .SumAsync(p => p.Amount);

        return Ok(new
        {
            message = "✅ Thanh toán đã được ghi nhận.",
            paymentId = payment.PaymentId,
            bookingCode = request.BookingCode,
            totalPaid
        });
    }


    // GET /api/payments?bookingCode=...  
    [HttpGet]
    public async Task<IActionResult> GetPayments([FromQuery] PaymentDto request)
    {
        if (string.IsNullOrEmpty(request.BookingCode))
            return BadRequest(new { message = "Mã booking không hợp lệ." });

        var booking = await _context.Bookings
            .FirstOrDefaultAsync(b => b.BookingCode == request.BookingCode);

        if (booking == null)
            return NotFound(new { message = "Không tìm thấy booking với mã này." });

        var payments = await _context.Payments
            .Where(p => p.BookingId == booking.BookingId)
            .OrderByDescending(p => p.PaidAt)
            .Select(p => new
            {
                p.PaymentId,
                p.Amount,
                p.PaymentMethod,
                p.TransactionRef,
                p.PaidAt
            })
            .ToListAsync();

        return Ok(payments);
    }



    // GET /api/payments/report?from=...&to=...
    [HttpGet("report")]
    public async Task<IActionResult> GetReport([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        if (from > to)
            return BadRequest(new { message = "Khoảng thời gian không hợp lệ." });

        var report = await _context.Payments
            .Where(p => p.PaidAt >= from && p.PaidAt <= to)
            .GroupBy(p => p.PaidAt.Date)
            .Select(g => new
            {
                Date = g.Key,
                TotalAmount = g.Sum(x => x.Amount),
                Count = g.Count()
            })
            .OrderBy(r => r.Date)
            .ToListAsync();

        var total = report.Sum(r => r.TotalAmount);

        return Ok(new
        {
            from,
            to,
            total,
            daily = report
        });
    }
}
