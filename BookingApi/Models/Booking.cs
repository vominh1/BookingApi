using System;
using System.Collections.Generic;

namespace BookingApi.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public string BookingCode { get; set; } = null!;

    public int? CustomerId { get; set; }

    public int? UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public decimal TotalAmount { get; set; }

    public string? Note { get; set; }

    public int StatusId { get; set; }

    public virtual ICollection<BookingItem> BookingItems { get; set; } = new List<BookingItem>();

    public virtual ICollection<BookingService> BookingServices { get; set; } = new List<BookingService>();

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual User? User { get; set; }

}

