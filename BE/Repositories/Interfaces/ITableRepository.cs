namespace BE.Repositories.Interfaces;
using BE.Models;
public interface ITableRepository
{
    Task<bool> CreateTableAsync( Table table );

    Task<bool> RemoveTableAsync( int tableId );

    Task<IEnumerable<Table>> GetAllTablesAsync(CancellationToken cancellationToken);

    Task<Table?> GetTableByIdAsync( int tableId );
}