using Microsoft.EntityFrameworkCore;
using BE.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){}
    public DbSet<Users> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Table> Tables { get; set; }
    public DbSet<FoodDrink> FoodDrinks { get; set; }
    public DbSet<Cat> Cats { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<BookingCat> BookingCats { get; set; }
    public DbSet<BookingDetail> BookingDetails { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<PaymentGatewayLog> PaymentGatewayLogs { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Save enum as string
        modelBuilder.Entity<Users>()
        .Property(e => e.Role)
        .HasConversion<string>();

        modelBuilder.Entity<Booking>()
        .Property(e => e.Status)
        .HasConversion<string>();

        modelBuilder.Entity<Payment>()
        .Property(p => p.Status)
        .HasConversion<string>();

        modelBuilder.Entity<PaymentGatewayLog>()
        .Property(l => l.Direction)
        .HasConversion<string>();

        // Make sure there's no same bookingID but has 2 same CatID or FoodDrinkID in it
        modelBuilder.Entity<BookingCat>()
            .HasKey(bc => new { bc.BookingID, bc.CatID });

        modelBuilder.Entity<BookingDetail>()
            .HasKey(bd => new { bd.BookingID, bd.FoodDrinkID });

        modelBuilder.Entity<Cat>( entity =>
        {
            entity.HasIndex(c => c.CatName).IsUnique();
        });
        modelBuilder.Entity<FoodDrink>( entity =>
        {
            entity.HasIndex(fd => fd.Name).IsUnique();
        });
        modelBuilder.Entity<Table>( entity =>
        {
            entity.HasIndex(t => t.TableName).IsUnique();
        });
        modelBuilder.Entity<Users>( entity =>
        {
            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
        });

        // default value for IsDeleted column in Booking table
        modelBuilder.Entity<Table>()
        .Property(t => t.IsActive)
        .HasDefaultValue(true);

        modelBuilder.Entity<FoodDrink>()
            .Property(f => f.IsActive)
            .HasDefaultValue(true);

        modelBuilder.Entity<Cat>()
            .Property(c => c.IsActive)
            .HasDefaultValue(true);

        // Unique constraint for Payment
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasIndex(p => p.OrderId).IsUnique();
            entity.HasIndex(p => p.RequestId).IsUnique();
        });


    }
}