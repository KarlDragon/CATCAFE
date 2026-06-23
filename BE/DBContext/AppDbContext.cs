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
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Users>()
        .Property(e => e.Role)
        .HasConversion<string>();

        // Make sure there's no same bookingID but has 2 same CatID or FoodDrinkID in it
        modelBuilder.Entity<BookingCat>()
            .HasKey(bc => new { bc.BookingID, bc.CatID });

        modelBuilder.Entity<BookingDetail>()
            .HasKey(bd => new { bd.BookingID, bd.FoodDrinkID });
    }
}