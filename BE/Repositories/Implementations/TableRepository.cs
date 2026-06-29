namespace BE.Repositories.Implementations;
using BE.Repositories.Interfaces;
using BE.Models;
using BE.DTOs;
using Microsoft.EntityFrameworkCore;

public class TableRepository : ITableRepository
{
    private readonly AppDbContext _context;

    public TableRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateTableAsync( Table table )
    {
        _context.Tables.Add(table);
        var affectedRow = await _context.SaveChangesAsync();
        return affectedRow > 0;
    }

    public async Task<bool> RemoveTableAsync( int tableId )
    {
        return await _context.Tables
        .Where(t => t.TableID == tableId)
        .ExecuteDeleteAsync() > 0;
    }

    public async Task<bool> UpdateTableAsync( UpdateTableDTO updateTableDTO)
    {
        var table = await _context.Tables.FindAsync(updateTableDTO.TableID);
        if ( table == null ) return false;

        table.TableName   = updateTableDTO.TableName ?? table.TableName;
        table.SeatAmount = updateTableDTO.SeatAmount ?? table.SeatAmount;

        var affectedRow = await _context.SaveChangesAsync();
        return affectedRow > 0;
    }
    public async Task<IEnumerable<Table>> GetAllTablesAsync( CancellationToken cancellationToken )
    {
        return await _context.Tables.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<bool> IsDuplicateName( string tableName )
    {
        return await _context.Tables.AnyAsync(tb => tb.TableName == tableName);
    } 
}