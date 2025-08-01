using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BoxCricketBooking.Data;
using BoxCricketBooking.Models;
using BoxCricketBooking.Services;

namespace BoxCricketBooking.Pages.Bookings
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaymentService _paymentService;

        public CreateModel(ApplicationDbContext context, IPaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }

        [BindProperty]
        public Booking Booking { get; set; } = new Booking();

        public BoxCricket? SelectedBoxCricket { get; set; }

        public async Task<IActionResult> OnGetAsync(int? boxId)
        {
            if (boxId.HasValue)
            {
                SelectedBoxCricket = await _context.BoxCrickets
                    .FirstOrDefaultAsync(b => b.Id == boxId && b.IsAvailable);

                if (SelectedBoxCricket == null)
                {
                    return RedirectToPage("/BoxCrickets/Index");
                }

                Booking.BoxCricketId = boxId.Value;
            }
            else
            {
                // If no box selected, redirect to boxes page
                return RedirectToPage("/BoxCrickets/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                SelectedBoxCricket = await _context.BoxCrickets
                    .FirstOrDefaultAsync(b => b.Id == Booking.BoxCricketId);
                return Page();
            }

            // Validate booking date
            if (Booking.BookingDate.Date < DateTime.Today)
            {
                ModelState.AddModelError("Booking.BookingDate", "Booking date cannot be in the past.");
                SelectedBoxCricket = await _context.BoxCrickets
                    .FirstOrDefaultAsync(b => b.Id == Booking.BoxCricketId);
                return Page();
            }

            // Validate time
            if (Booking.StartTime >= Booking.EndTime)
            {
                ModelState.AddModelError("Booking.EndTime", "End time must be after start time.");
                SelectedBoxCricket = await _context.BoxCrickets
                    .FirstOrDefaultAsync(b => b.Id == Booking.BoxCricketId);
                return Page();
            }

            // Get the selected box cricket
            var boxCricket = await _context.BoxCrickets
                .FirstOrDefaultAsync(b => b.Id == Booking.BoxCricketId);

            if (boxCricket == null || !boxCricket.IsAvailable)
            {
                ModelState.AddModelError("", "Selected cricket box is not available.");
                SelectedBoxCricket = boxCricket;
                return Page();
            }

            // Validate number of players
            if (Booking.NumberOfPlayers > boxCricket.MaxPlayers)
            {
                ModelState.AddModelError("Booking.NumberOfPlayers", $"Maximum {boxCricket.MaxPlayers} players allowed for this box.");
                SelectedBoxCricket = boxCricket;
                return Page();
            }

            // Calculate total amount
            var duration = Booking.EndTime - Booking.StartTime;
            var hours = (decimal)duration.TotalHours;
            Booking.TotalAmount = hours * boxCricket.PricePerHour;

            // Check for booking conflicts
            var conflictingBooking = await _context.Bookings
                .Where(b => b.BoxCricketId == Booking.BoxCricketId &&
                           b.BookingDate == Booking.BookingDate &&
                           b.Status != BookingStatus.Cancelled &&
                           ((b.StartTime <= Booking.StartTime && b.EndTime > Booking.StartTime) ||
                            (b.StartTime < Booking.EndTime && b.EndTime >= Booking.EndTime) ||
                            (b.StartTime >= Booking.StartTime && b.EndTime <= Booking.EndTime)))
                .FirstOrDefaultAsync();

            if (conflictingBooking != null)
            {
                ModelState.AddModelError("", "This time slot is already booked. Please select a different time.");
                SelectedBoxCricket = boxCricket;
                return Page();
            }

            // Create payment intent
            try
            {
                var paymentIntentId = await _paymentService.CreatePaymentIntentAsync(Booking.TotalAmount);
                Booking.StripePaymentIntentId = paymentIntentId;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Payment service is currently unavailable. Please try again later.");
                SelectedBoxCricket = boxCricket;
                return Page();
            }

            // Save booking
            _context.Bookings.Add(Booking);
            await _context.SaveChangesAsync();

            // Redirect to payment page
            return RedirectToPage("/Bookings/Payment", new { id = Booking.Id });
        }
    }
}