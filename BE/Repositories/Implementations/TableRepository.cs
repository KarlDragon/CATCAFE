namespace BE.Repositories.Implementations;
using BE.Repositories.Interfaces;
using BE.Models;
using Microsoft.EntityFrameworkCore;

public class TableRepository : ITableRepository
{
    private readonly AppDbContext _context;

    public TableRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateTablesAsync( Table table )
    {
        _context.Tables.Add(table);
        var affectedRow = await _context.SaveChangesAsync();
        return affectedRow > 0;
    }

    public async Task<bool> RemoveTableAsync( int tableId )
    {
        var table = await GetTableByIdAsync(tableId);
        if (table != null)
        {
            _context.Tables.Remove(table);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<Table>> GetAllTablesAsync( CancellationToken cancellationToken )
    {
        return await _context.Tables.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Table?> GetTableByIdAsync( int tableId )
    {
        return await _context.Tables.FirstOrDefaultAsync(tb => tb.TableID == tableId);
    } 
}