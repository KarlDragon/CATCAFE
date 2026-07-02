namespace BE.Controllers;
using BE.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using BE.DTOs;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]

public class TableController : ControllerBase
{
    private readonly ITableService _tableService;

    public TableController(ITableService tableService)
    {
        _tableService = tableService;
    }

    [HttpPost]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> CreateTable([FromBody] CreateTableDTO createTableDTO)
    {
        bool isCreated = await _tableService.CreateTableAsync(createTableDTO);
        if (!isCreated)
        {
            return BadRequest(new { message = "Failed to create table." });
        }
        return Ok(new { message = "Table created successfully." });
    }

    [HttpDelete("{tableId}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> RemoveTable(int tableId)
    {
        await _tableService.RemoveTableAsync(tableId);
        return Ok(new { message = "Table removed successfully." });
    }   

    [HttpPut]
    [Authorize(Roles = "Owner, Staff")]
    public async Task<IActionResult> UpdateTable([FromBody] UpdateTableDTO updateTableDTO)
    {
        bool isUpdated = await _tableService.UpdateTableAsync(updateTableDTO);
        if (!isUpdated)
        {
            return BadRequest(new { message = "Failed to update table." });
        }
        return Ok(new { message = "Table updated successfully." });
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllTables(CancellationToken cancellationToken)
    {
        var tables = await _tableService.GetAllTablesAsync(cancellationToken);
        return Ok(tables);
    }
}