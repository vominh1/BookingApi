using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingApi.Models
{
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

        public string? Location { get; set; }

        public decimal? Price { get; set; }

        // ✅ Navigation property sang Venue (nếu có)
        public virtual Venue? Venue { get; set; }

        // ✅ Navigation property sang bảng AvailabilityOverride
        public virtual ICollection<AvailabilityOverride> AvailabilityOverrides { get; set; } = new List<AvailabilityOverride>();
        // ✅ Quan hệ với BookingItem
        public virtual ICollection<BookingItem> BookingItems { get; set; } = new List<BookingItem>();
    }
}
