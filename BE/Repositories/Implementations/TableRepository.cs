namespace BE.Repositories.Implementations;
using BE.Repositories.Interfaces;
using BE.Models;
using BE.DTOs;
using Microsoft.EntityFrameworkCore;

public class TableRepository : ITableRepository
{
    private readonly AppDbContext _context;
    private readonly Logger<TableRepository> _logger;

    public TableRepository(AppDbContext context, Logger<TableRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> CreateTableAsync( Table table )
    {
        _context.Tables.Add(table);
        var affectedRow = await _context.SaveChangesAsync();
        return affectedRow > 0;
    }

    public async Task<bool> RemoveTableAsync( int tableId )
    {
        var table = await GetTableByIdAsync(tableId);
        _context.Tables.Remove(table);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateTableAsync( UpdateTableDTO updateTableDTO)
    {
        var table = await GetTableByIdAsync(updateTableDTO.TableID);

        table.TableName   = updateTableDTO.TableName ?? table.TableName;
        table.SeatAmount = updateTableDTO.SeatAmount ?? table.SeatAmount;

        var affectedRow = await _context.SaveChangesAsync();
        return affectedRow > 0;
    }
    public async Task<IEnumerable<Table>> GetAllTablesAsync( CancellationToken cancellationToken )
    {
        return await _context.Tables.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Table> GetTableByIdAsync( int tableId )
    {
        var table = await _context.Tables.FirstOrDefaultAsync(tb => tb.TableID == tableId);
        if ( table == null)
        {
            _logger.LogInformation("Table's not existed {tableID}", tableId);
            throw new UnauthorizedAccessException();
        }
        return table;
    } 
}