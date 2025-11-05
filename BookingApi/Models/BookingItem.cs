using System;
using System.Collections.Generic;

namespace BookingApi.Models;

public partial class BookingItem
{
    public int BookingItemId { get; set; }

    public int BookingId { get; set; }

    public int ResourceId { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public string? Note { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Resource Resource { get; set; } = null!;
}
