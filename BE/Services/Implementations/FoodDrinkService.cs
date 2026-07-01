namespace BE.Services.Implementations;
using BE.Services.Interfaces;
using BE.Models;
using BE.DTOs;
using BE.Repositories.Interfaces;
using BE.Exceptions;
public class FoodDrinkService : IFoodDrinkService
{
    private readonly IFoodDrinkRepository _foodDrinkRepository;

    public FoodDrinkService( IFoodDrinkRepository foodDrinkRepository )
    {
        _foodDrinkRepository = foodDrinkRepository;
    }

    public async Task<bool> CreateFoodDrinkAsync( CreateFoodDrinkDTO createFoodDrinkDto )
    {
        var newFoodDrink = new FoodDrink
        {
            Name = createFoodDrinkDto.Name.Trim(),
            Price = createFoodDrinkDto.Price,
            Quantity = createFoodDrinkDto.Quantity
        };
        var duplicateName = await _foodDrinkRepository.IsDuplicateName(newFoodDrink.Name);
        if (duplicateName)
        {
            throw new DuplicateNameException("Trùng tên món ăn/đồ uống");
        }

        return await _foodDrinkRepository.CreateFoodDrinkAsync(newFoodDrink);
    }

    public async Task RemoveFoodDrinkAsync( int foodDrinkId)
    {
        var result = await _foodDrinkRepository.RemoveFoodDrinkAsync(foodDrinkId);
        if ( !result )
        {
            throw new NotFoundException($"Can't remove food drink {foodDrinkId}");
        }
    }

    public async Task<bool> UpdateFoodDrinkAsync( UpdateFoodDrinkDTO updateFoodDrinkDTO)
    {
        if (updateFoodDrinkDTO.Name != null) updateFoodDrinkDTO.Name = updateFoodDrinkDTO.Name?.Trim();
        return await _foodDrinkRepository.UpdateFoodDrinkAsync(updateFoodDrinkDTO);
    }

    public async Task<IEnumerable<FoodDrink>> GetAllFoodDrinksAsync(CancellationToken cancellationToken)
    {
        return await _foodDrinkRepository.GetAllFoodDrinksAsync(cancellationToken);
    }
}