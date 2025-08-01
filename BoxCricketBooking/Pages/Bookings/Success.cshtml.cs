using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BoxCricketBooking.Data;
using BoxCricketBooking.Models;
using BoxCricketBooking.Services;

namespace BoxCricketBooking.Pages.Bookings
{
    public class SuccessModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IPaymentService _paymentService;

        public SuccessModel(ApplicationDbContext context, IPaymentService paymentService)
        {
            _context = context;
            _paymentService = paymentService;
        }

        public Booking? Booking { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Booking = await _context.Bookings
                .Include(b => b.BoxCricket)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (Booking == null)
            {
                return RedirectToPage("/BoxCrickets/Index");
            }

            // If booking is still pending, confirm the payment
            if (Booking.Status == BookingStatus.Pending && !string.IsNullOrEmpty(Booking.StripePaymentIntentId))
            {
                var paymentConfirmed = await _paymentService.ConfirmPaymentAsync(Booking.StripePaymentIntentId);
                
                if (paymentConfirmed)
                {
                    Booking.Status = BookingStatus.Confirmed;
                    Booking.UpdatedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Payment failed, redirect to payment page
                    return RedirectToPage("/Bookings/Payment", new { id = Booking.Id });
                }
            }

            return Page();
        }
    }
}