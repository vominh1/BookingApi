using BookingApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
namespace BookingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingApi.Models.BookingDBContext _context;

        public BookingController(BookingApi.Models.BookingDBContext context)
        {
            _context = context;
        }


        // Lấy danh sách booking
        [HttpGet]

        public IActionResult GetAll(int offset = 0, int pageSize =7)
        {
            var query = from bk in _context.Bookings
                        join st in _context.Status on bk.StatusId equals st.StatusId
                        orderby bk.CreatedAt descending   // sắp xếp mới nhất
                        select new
                        {
                            bk.BookingId,
                            bk.CustomerId,
                            bk.BookingCode,
                            bk.CreatedAt,
                            bk.StatusId,
                            StatusName = st.StatusName,
                            bk.TotalAmount
                        };

            var total = query.Count();

            var bookings = query
                .Skip(offset)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                total,
                offset,
                pageSize,
                items = bookings
            });
        }


        // xem chi tiết booking theo mã booking
        [HttpGet("bycode/{bookingCode}")]
        public IActionResult GetBookingByCode(string bookingCode)
        {
            try
            {
                var result = _context.Database
                    .SqlQueryRaw<BookingDetailDTO>("EXEC sp_GetBookingDetailByCode {0}", bookingCode)
                    .AsEnumerable()
                    .FirstOrDefault();

                if (result == null)
                    return NotFound(new { message = "Không tìm thấy booking." });

                // 🔹 Lấy danh sách dịch vụ đi kèm
                var services = (from bs in _context.BookingServices
                                join s in _context.Services on bs.ServiceId equals s.ServiceId
                                where bs.BookingId == result.BookingId
                                select new
                                {
                                    s.ServiceId,
                                    s.Name,
                                    s.Price,
                                    bs.Quantity,
                                    bs.UnitPrice
                                }).ToList();

                return Ok(new
                {
                    Booking = result,
                    Services = services
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }





        // Tạo mới booking bằng stored procedure    
        [HttpPost("byform")]
        public IActionResult CreateBooking([FromBody] BookingFormDto dto)
        {
            string bookingCode = "BK" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(100, 999);
            if (dto.CustomerId == null || dto.UserId == null)
            {
                return BadRequest(new { message = "CustomerId and UserId are required." });
            }

            var bookingId = _context.Database
                .SqlQueryRaw<int>(
                    "EXEC CreateBooking @BookingCode = {0}, @CustomerId = {1}, @UserId = {2}, @Note = {3}",
                    bookingCode,
                    dto.CustomerId,
                    dto.UserId,
                    dto.Note ?? string.Empty
                )
                .AsEnumerable()
                .FirstOrDefault();

            return Ok(new
            {
                Message = "✅ Tạo booking thành công!",
                BookingId = bookingId,
                BookingCode = bookingCode,
                Note = dto.Note
            });
        }



        // tạo mới booking
        [HttpPost]
        public IActionResult Create([FromBody] Booking booking)
        {
            // 🔹 Sinh mã booking
            booking.BookingCode = "BK" + DateTime.Now.ToString("yyyyMMdd") +
                                  new Random().Next(1000, 9999);

            try
            {
                // Gọi stored procedure CreateBooking
                var bookingId = _context.Database.SqlQuery<int>(
                    $"EXEC CreateBooking @BookingCode = {booking.BookingCode}, @CustomerId = {booking.CustomerId}, @UserId = {booking.UserId}, @Note = {booking.Note ?? ""}"
                ).AsEnumerable().FirstOrDefault();

                booking.BookingId = bookingId;
                booking.CreatedAt = DateTime.Now;

                return Ok(new
                {
                    Message = "Tạo booking thành công!",
                    BookingId = bookingId,
                    BookingCode = booking.BookingCode
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi tạo booking: {ex.Message}");
            }
        }





        //// Cập nhật booking
        //[HttpPut("{id}")]
        //public IActionResult Update(int id, [FromBody] Booking updated)
        //{
        //    var booking = _context.Bookings.Find(id);
        //    if (booking == null)
        //        return NotFound();

        //    booking.CustomerId = updated.CustomerId;
        //    booking.RoomId = updated.RoomId;
        //    booking.Date = updated.Date;
        //    booking.Status = updated.Status;

        //    _context.SaveChanges();
        //    return Ok(booking);
        //}

        // Xóa booking
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null)
                return NotFound();

            _context.Bookings.Remove(booking);
            _context.SaveChanges();
            return NoContent();
        }

        // Kiểm tra mã booking tồn tại
        [HttpGet("bycode2/{bookingCode}")]
        public IActionResult GetBookingByCode2(string bookingCode)  
        {
            var booking = _context.Bookings.FirstOrDefault(b => b.BookingCode == bookingCode);
            if (booking == null)
                return NotFound(new { message = "Không tìm thấy mã booking." });

            return Ok(new { message = "✅ Booking tồn tại", booking.BookingId });
        }

    }


}
