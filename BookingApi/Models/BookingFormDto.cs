namespace BookingApi.Models
{
    public class BookingFormDto
    {
        public int? CustomerId { get; set; }
        public int? UserId { get; set; }
        public string? Note { get; set; }
    }
    public class BookingItemDto
    {
        public int ResourceId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? Note { get; set; }
    }


}
