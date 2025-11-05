using System;
using System.Collections.Generic;

namespace BookingApi.Models;

public partial class Venue
{
    public int VenueId { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string? Description { get; set; }

    public string? TimeZone { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Resource> Resources { get; set; } = new List<Resource>();
}
