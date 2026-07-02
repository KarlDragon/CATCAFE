namespace BE.Repositories.Implementations;
using BE.Repositories.Interfaces;
using BE.Models;
using BE.DTOs;
using Microsoft.EntityFrameworkCore;

public class FoodDrinkRepository : IFoodDrinkRepository
{
    private readonly AppDbContext _context;

    public FoodDrinkRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateFoodDrinkAsync( FoodDrink foodDrink )
    {
        _context.FoodDrinks.Add(foodDrink);
        var affectedRow = await _context.SaveChangesAsync();
        return affectedRow > 0;
    }

    public async Task<bool> RemoveFoodDrinkAsync( int foodDrinkId )
    {
        return await _context.FoodDrinks
        .Where(fd => fd.FoodDrinkID == foodDrinkId)
        .ExecuteDeleteAsync() > 0;
    }

    public async Task<bool> UpdateFoodDrinkAsync( UpdateFoodDrinkDTO updateFoodDrinkDTO)
    {
        var foodDrink = await _context.FoodDrinks.FindAsync(updateFoodDrinkDTO.FoodDrinkID);
        if ( foodDrink == null ) return false;

        foodDrink.Name = updateFoodDrinkDTO.Name ?? foodDrink.Name;
        foodDrink.Price = updateFoodDrinkDTO.Price ?? foodDrink.Price;
        foodDrink.Quantity = updateFoodDrinkDTO.Quantity ?? foodDrink.Quantity;

        var affectedRow = await _context.SaveChangesAsync();
        return affectedRow > 0;
    }

    public async Task<IEnumerable<FoodDrink>> GetAllFoodDrinksAsync( CancellationToken cancellationToken )
    {
        return await _context.FoodDrinks.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<bool> IsDuplicateName( string foodDrinkName )
    {
        return await _context.FoodDrinks.AnyAsync(fd => fd.Name == foodDrinkName);
    }

    public async Task<decimal> GetFoodDrinkPriceByIdAsync( int foodDrinkId )
    {
        var foodDrink = await _context.FoodDrinks.FindAsync(foodDrinkId);
        if (foodDrink == null) throw new ArgumentException($"FoodDrink with ID {foodDrinkId} not found.");
        return foodDrink.Price;
    }

}