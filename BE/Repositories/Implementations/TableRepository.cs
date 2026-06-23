namespace BE.Repositories.Implementations;
using BE.Repositories.Interfaces;
using BE.Models;
using Microsoft.EntityFrameworkCore;

public class TableRepository : ITableRepository
{
    private readonly AppDbContext _context;

    public TableRepository( AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateTablesAsync( int tableAmount)
    {
        if ( tableAmount <= 0 ) return false;

        var tables = Enumerable.Range(1, tableAmount).Select(_ => new Table()).ToList();

        _context.Tables.AddRange(tables);

        var result = await _context.SaveChangesAsync();
        return result == tableAmount;
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

    public async Task<bool> BookTableAsync ( Table table )
    {
        var result = await _context.Tables
        .Where(tb => tb.TableID == table.TableID && tb.UserID == null) 
        .ExecuteUpdateAsync(set => set
            .SetProperty(tb => tb.UserID, table.UserID)
            .SetProperty(tb => tb.BookedTime, table.BookedTime));

        return result > 0;
    }

    public async Task<bool> ClearTableAsync( int tableId )
    {
        var result = await _context.Tables.Where( tb => tb.TableID == tableId)
                                          .ExecuteUpdateAsync( set => set.SetProperty( tb => tb.UserID, (int?)null)
                                                                       .SetProperty( tb => tb.BookedTime, (DateTime?) null));

        return result > 0;
    }

    public async Task<IEnumerable<Table>> GetAllTablesAsync(CancellationToken cancellationToken)
    {
        return await _context.Tables.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Table>> GetUserTablesAsync( int userId, CancellationToken cancellationToken )
    {
        return await _context.Tables.Where( tb => tb.UserID == userId ).AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Table?> GetTableByIdAsync( int tableId)
    {
        return await _context.Tables.FirstOrDefaultAsync(tb => tb.TableID == tableId);
    }

}