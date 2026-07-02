namespace BE.Controllers;
using BE.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BE.DTOs;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]

public class CatController : ControllerBase
{
    private readonly ICatService _catService;

    public CatController(ICatService catService)
    {
        _catService = catService;
    }

    [HttpPost]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> CreateCat([FromBody] CreateCatDTO createCatDTO)
    {
        bool isCreated = await _catService.CreateCatAsync(createCatDTO);
        if (!isCreated)
        {
            return BadRequest(new { message = "Failed to create cat." });
        }
        return Ok(new { message = "Cat created successfully." });
    }

    [HttpDelete("{catId}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> RemoveCat(int catId)
    {
        await _catService.RemoveCatAsync(catId);
        return Ok(new { message = "Cat removed successfully." });
    }

    [HttpPut]
    [Authorize(Roles = "Owner,Staff")]
    public async Task<IActionResult> UpdateCat([FromBody] UpdateCatDTO updateCatDTO)
    {
        bool isUpdated = await _catService.UpdateCatAsync(updateCatDTO);
        if (!isUpdated)
        {
            return BadRequest(new { message = "Failed to update cat." });
        }
        return Ok(new { message = "Cat updated successfully." });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllCats(CancellationToken cancellationToken)
    {
        var cats = await _catService.GetAllCatsAsync(cancellationToken);
        return Ok(cats);
    }
}