using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BoxCricketBooking.Data;
using BoxCricketBooking.Models;

namespace BoxCricketBooking.Pages.Admin
{
    public class BookingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BookingsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Booking> Bookings { get; set; } = new List<Booking>();

        public async Task OnGetAsync()
        {
            Bookings = await _context.Bookings
                .Include(b => b.BoxCricket)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();
        }
    }
}