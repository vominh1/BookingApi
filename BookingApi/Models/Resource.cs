using System;
using System.Collections.Generic;

namespace BookingApi.Models;

public partial class Resource
{
    public int ResourceId { get; set; }

    public int VenueId { get; set; }

    public string? Code { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? Capacity { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<AvailabilityOverride> AvailabilityOverrides { get; set; } = new List<AvailabilityOverride>();

    public virtual ICollection<BookingItem> BookingItems { get; set; } = new List<BookingItem>();

    public virtual Venue Venue { get; set; } = null!;
}
