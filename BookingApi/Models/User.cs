using System;
using System.Collections.Generic;

namespace BookingApi.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
