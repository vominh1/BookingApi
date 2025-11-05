    namespace BookingApi.Models
    {
        public class BookingDetails
        {

        }
        public class BookingDetailDTO
        {
            public int BookingId { get; set; }
            public  required string BookingCode { get; set; }
            public DateTime CreatedAt { get; set; }
            public int StatusId { get; set; }
            public required string StatusName { get; set; }

        public int CustomerId { get; set; }
            public required string FullName { get; set; }
        public required string Email { get; set; }
            public required string Phone { get; set; }
        }

    }
