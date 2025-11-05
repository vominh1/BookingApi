using System;
using System.Collections.Generic;

namespace BookingApi.Models;

public partial class AuditLog
{
    public long AuditId { get; set; }

    public int? UserId { get; set; }

    public string Action { get; set; } = null!;

    public string? Entity { get; set; }

    public string? EntityId { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? Details { get; set; }
}
