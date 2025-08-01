using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BoxCricketBooking.Data;
using BoxCricketBooking.Models;

namespace BoxCricketBooking.Pages.BoxCrickets
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<BoxCricket> BoxCrickets { get; set; } = new List<BoxCricket>();

        public async Task OnGetAsync()
        {
            BoxCrickets = await _context.BoxCrickets
                .Where(b => b.IsAvailable)
                .OrderBy(b => b.PricePerHour)
                .ToListAsync();
        }
    }
}