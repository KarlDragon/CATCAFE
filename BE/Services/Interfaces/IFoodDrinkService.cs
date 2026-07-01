namespace BE.Services.Interfaces;
using BE.Models;
using BE.DTOs;

public interface IFoodDrinkService
{
    Task<bool> CreateFoodDrinkAsync( CreateFoodDrinkDTO createFoodDrinkDto );

    Task RemoveFoodDrinkAsync( int foodDrinkId );

    Task<bool> UpdateFoodDrinkAsync( UpdateFoodDrinkDTO updateFoodDrinkDTO );

    Task<IEnumerable<FoodDrink>> GetAllFoodDrinksAsync(CancellationToken cancellationToken);
}