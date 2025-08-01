using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using BoxCricketBooking.Data;
using BoxCricketBooking.Models;

namespace BoxCricketBooking.Pages.Bookings
{
    public class PaymentModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public PaymentModel(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public Booking? Booking { get; set; }
        public IConfiguration Configuration => _configuration;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Booking = await _context.Bookings
                .Include(b => b.BoxCricket)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (Booking == null)
            {
                return RedirectToPage("/BoxCrickets/Index");
            }

            // Check if booking is already confirmed
            if (Booking.Status == BookingStatus.Confirmed)
            {
                return RedirectToPage("/Bookings/Success", new { id = Booking.Id });
            }

            // Check if booking is cancelled
            if (Booking.Status == BookingStatus.Cancelled)
            {
                return RedirectToPage("/Bookings/Cancelled", new { id = Booking.Id });
            }

            return Page();
        }
    }
}