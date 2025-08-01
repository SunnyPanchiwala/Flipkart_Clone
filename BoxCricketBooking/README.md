# Box Cricket Booking System

A modern web application for booking cricket matches in indoor cricket boxes, built with ASP.NET Core, Entity Framework, and Stripe payment integration.

## Features

- **Cricket Box Management**: View and manage different cricket boxes with varying capacities and pricing
- **Online Booking**: Easy-to-use booking system with real-time availability checking
- **Payment Integration**: Secure payment processing using Stripe
- **Responsive Design**: Modern, mobile-friendly UI built with Bootstrap 5
- **Booking Management**: Track booking status and manage reservations
- **Conflict Prevention**: Automatic detection of booking conflicts
- **Email Notifications**: Booking confirmations and updates

## Technology Stack

- **Backend**: ASP.NET Core 8.0
- **Database**: SQL Server with Entity Framework Core
- **Frontend**: Razor Pages with Bootstrap 5
- **Payment**: Stripe API integration
- **Icons**: Font Awesome 6.0

## Prerequisites

- .NET 8.0 SDK
- SQL Server (LocalDB or full SQL Server)
- Stripe account for payment processing

## Installation

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd BoxCricketBooking
   ```

2. **Install dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure the database**
   - Update the connection string in `appsettings.json`
   - Run the following command to create the database:
   ```bash
   dotnet ef database update
   ```

4. **Configure Stripe**
   - Sign up for a Stripe account at https://stripe.com
   - Get your API keys from the Stripe Dashboard
   - Update the Stripe configuration in `appsettings.json`:
   ```json
   {
     "Stripe": {
       "SecretKey": "sk_test_your_stripe_secret_key_here",
       "PublishableKey": "pk_test_your_stripe_publishable_key_here"
     }
   }
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

6. **Access the application**
   - Open your browser and navigate to `https://localhost:5001` or `http://localhost:5000`

## Project Structure

```
BoxCricketBooking/
├── Data/
│   └── ApplicationDbContext.cs          # Entity Framework context
├── Models/
│   ├── BoxCricket.cs                   # Cricket box entity
│   └── Booking.cs                      # Booking entity
├── Pages/
│   ├── Index.cshtml                    # Homepage
│   ├── BoxCrickets/
│   │   └── Index.cshtml                # Cricket boxes listing
│   └── Bookings/
│       ├── Create.cshtml               # Booking creation form
│       ├── Payment.cshtml              # Payment processing
│       └── Success.cshtml              # Booking confirmation
├── Services/
│   └── PaymentService.cs               # Stripe payment service
├── wwwroot/                            # Static files
└── Program.cs                          # Application entry point
```

## Database Schema

### BoxCricket Table
- `Id` (Primary Key)
- `Name` - Cricket box name
- `Description` - Box description
- `PricePerHour` - Hourly rate
- `MaxPlayers` - Maximum number of players
- `Location` - Box location
- `ImageUrl` - Box image URL
- `IsAvailable` - Availability status

### Booking Table
- `Id` (Primary Key)
- `BoxCricketId` (Foreign Key)
- `CustomerName` - Customer's full name
- `CustomerEmail` - Customer's email
- `CustomerPhone` - Customer's phone number
- `BookingDate` - Date of the match
- `StartTime` - Start time
- `EndTime` - End time
- `NumberOfPlayers` - Number of players
- `TotalAmount` - Total booking amount
- `SpecialRequirements` - Any special requests
- `Status` - Booking status (Pending, Confirmed, Cancelled, Completed)
- `StripePaymentIntentId` - Stripe payment intent ID
- `CreatedAt` - Booking creation timestamp
- `UpdatedAt` - Last update timestamp

## API Endpoints

The application uses Razor Pages for the UI, but you can extend it with Web API controllers for mobile apps or third-party integrations.

## Payment Flow

1. User selects a cricket box and fills booking details
2. System validates availability and calculates total amount
3. Stripe payment intent is created
4. User completes payment using Stripe Elements
5. Payment is confirmed and booking status is updated
6. User receives booking confirmation

## Testing

For testing payments, use Stripe's test card numbers:
- **Success**: 4242 4242 4242 4242
- **Decline**: 4000 0000 0000 0002
- **Requires Authentication**: 4000 0025 0000 3155

## Deployment

### Local Development
```bash
dotnet run
```

### Production Deployment
1. Update connection strings for production database
2. Configure production Stripe keys
3. Set environment variables for sensitive data
4. Deploy to your preferred hosting platform (Azure, AWS, etc.)

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For support and questions:
- Email: support@boxcricket.com
- Phone: +1 (234) 567-8900

## Future Enhancements

- User authentication and profiles
- Admin dashboard for managing bookings
- Email notifications
- Mobile app integration
- Advanced booking analytics
- Equipment rental system
- Tournament management
- Loyalty program