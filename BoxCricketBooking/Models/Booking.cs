using System.ComponentModel.DataAnnotations;

namespace BoxCricketBooking.Models
{
    public class Booking
    {
        public int Id { get; set; }
        
        [Required]
        public int BoxCricketId { get; set; }
        
        public BoxCricket BoxCricket { get; set; } = null!;
        
        [Required]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;
        
        [Required]
        [Phone]
        public string CustomerPhone { get; set; } = string.Empty;
        
        [Required]
        public DateTime BookingDate { get; set; }
        
        [Required]
        public TimeSpan StartTime { get; set; }
        
        [Required]
        public TimeSpan EndTime { get; set; }
        
        [Required]
        public int NumberOfPlayers { get; set; }
        
        [Required]
        public decimal TotalAmount { get; set; }
        
        public string? SpecialRequirements { get; set; }
        
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        
        public string? StripePaymentIntentId { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
    }
    
    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled,
        Completed
    }
}