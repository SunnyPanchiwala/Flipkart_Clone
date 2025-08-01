using System.ComponentModel.DataAnnotations;

namespace BoxCricketBooking.Models
{
    public class BoxCricket
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public decimal PricePerHour { get; set; }
        
        [Required]
        public int MaxPlayers { get; set; }
        
        [Required]
        public string Location { get; set; } = string.Empty;
        
        public string? ImageUrl { get; set; }
        
        public bool IsAvailable { get; set; } = true;
        
        public List<Booking> Bookings { get; set; } = new List<Booking>();
    }
}