namespace BE.Controllers;
using BE.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BE.DTOs;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]

public class FoodDrinkController : ControllerBase
{
    private readonly IFoodDrinkService _foodDrinkService;

    public FoodDrinkController(IFoodDrinkService foodDrinkService)
    {
        _foodDrinkService = foodDrinkService;
    }

    [HttpPost]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> CreateFoodDrink([FromBody] CreateFoodDrinkDTO createFoodDrinkDTO)
    {
        bool isCreated = await _foodDrinkService.CreateFoodDrinkAsync(createFoodDrinkDTO);
        if (!isCreated)
        {
            return BadRequest(new { message = "Failed to create food/drink." });
        }
        return Ok(new { message = "Food/drink created successfully." });
    }

    [HttpDelete("{foodDrinkId}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> RemoveFoodDrink(int foodDrinkId)
    {
        await _foodDrinkService.RemoveFoodDrinkAsync(foodDrinkId);
        return Ok(new { message = "Food/drink removed successfully." });
    }   

    [HttpPut]
    [Authorize(Roles = "Owner,Staff")]
    public async Task<IActionResult> UpdateFoodDrink([FromBody] UpdateFoodDrinkDTO updateFoodDrinkDTO)
    {
        bool isUpdated = await _foodDrinkService.UpdateFoodDrinkAsync(updateFoodDrinkDTO);
        if (!isUpdated)
        {
            return BadRequest(new { message = "Failed to update food/drink." });
        }
        return Ok(new { message = "Food/drink updated successfully." });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllFoodDrinks(CancellationToken cancellationToken)
    {
        var foodDrinks = await _foodDrinkService.GetAllFoodDrinksAsync(cancellationToken);
        return Ok(foodDrinks);
    }
}