using Microsoft.EntityFrameworkCore;
using BoxCricketBooking.Models;

namespace BoxCricketBooking.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<BoxCricket> BoxCrickets { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure BoxCricket entity
            modelBuilder.Entity<BoxCricket>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
                entity.Property(e => e.PricePerHour).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Location).IsRequired();
            });

            // Configure Booking entity
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CustomerEmail).IsRequired();
                entity.Property(e => e.CustomerPhone).IsRequired();
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                
                entity.HasOne(e => e.BoxCricket)
                    .WithMany(e => e.Bookings)
                    .HasForeignKey(e => e.BoxCricketId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Seed data for BoxCrickets
            modelBuilder.Entity<BoxCricket>().HasData(
                new BoxCricket
                {
                    Id = 1,
                    Name = "Premium Cricket Box A",
                    Description = "High-quality cricket box with professional equipment and excellent facilities. Perfect for serious cricket enthusiasts.",
                    PricePerHour = 1500.00m,
                    MaxPlayers = 12,
                    Location = "Sports Complex, Downtown",
                    ImageUrl = "/images/box-a.jpg",
                    IsAvailable = true
                },
                new BoxCricket
                {
                    Id = 2,
                    Name = "Standard Cricket Box B",
                    Description = "Standard cricket box with good facilities and equipment. Ideal for casual players and groups.",
                    PricePerHour = 1200.00m,
                    MaxPlayers = 10,
                    Location = "Sports Complex, Downtown",
                    ImageUrl = "/images/box-b.jpg",
                    IsAvailable = true
                },
                new BoxCricket
                {
                    Id = 3,
                    Name = "Family Cricket Box C",
                    Description = "Family-friendly cricket box with safety features and equipment suitable for all ages.",
                    PricePerHour = 1000.00m,
                    MaxPlayers = 8,
                    Location = "Sports Complex, Downtown",
                    ImageUrl = "/images/box-c.jpg",
                    IsAvailable = true
                }
            );
        }
    }
}