namespace BE.Repositories.Interfaces;
using BE.Models;
using BE.DTOs;
public interface IFoodDrinkRepository
{
    Task<bool> CreateFoodDrinkAsync( FoodDrink foodDrink );

    Task<bool> RemoveFoodDrinkAsync( int foodDrinkId );

    Task<bool> UpdateFoodDrinkAsync( UpdateFoodDrinkDTO updateFoodDrinkDTO );

    Task<IEnumerable<FoodDrink>> GetAllFoodDrinksAsync(CancellationToken cancellationToken);

    Task<bool> IsDuplicateName( string foodDrinkName );

    Task<decimal> GetFoodDrinkPriceByIdAsync( int foodDrinkId );
}