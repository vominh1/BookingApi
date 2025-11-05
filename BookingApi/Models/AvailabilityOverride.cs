using System;
using System.Collections.Generic;

namespace BookingApi.Models;

public partial class AvailabilityOverride
{
    public int OverrideId { get; set; }

    public int ResourceId { get; set; }

    public DateTime StartAt { get; set; }

    public DateTime EndAt { get; set; }

    public bool IsAvailable { get; set; }

    public string? Note { get; set; }

    public virtual Resource Resource { get; set; } = null!;
}
