using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BoxCricketBooking.Data;
using BoxCricketBooking.Models;
using Stripe;

namespace BoxCricketBooking.Pages.Webhooks
{
    public class StripeModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public StripeModel(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _configuration["Stripe:WebhookSecret"]
                );

                // Handle the event
                switch (stripeEvent.Type)
                {
                    case Events.PaymentIntentSucceeded:
                        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                        await HandlePaymentSuccess(paymentIntent);
                        break;
                    case Events.PaymentIntentPaymentFailed:
                        var failedPayment = stripeEvent.Data.Object as PaymentIntent;
                        await HandlePaymentFailure(failedPayment);
                        break;
                    default:
                        // Unexpected event type
                        break;
                }

                return new OkResult();
            }
            catch (StripeException e)
            {
                return new BadRequestResult();
            }
        }

        private async Task HandlePaymentSuccess(PaymentIntent paymentIntent)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.StripePaymentIntentId == paymentIntent.Id);

            if (booking != null)
            {
                booking.Status = BookingStatus.Confirmed;
                booking.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }

        private async Task HandlePaymentFailure(PaymentIntent paymentIntent)
        {
            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.StripePaymentIntentId == paymentIntent.Id);

            if (booking != null)
            {
                booking.Status = BookingStatus.Cancelled;
                booking.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
        }
    }
}