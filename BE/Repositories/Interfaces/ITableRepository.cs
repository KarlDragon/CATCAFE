namespace BE.Repositories.Interfaces;
using BE.Models;
using BE.DTOs;
public interface ITableRepository
{
    Task<bool> CreateTableAsync( Table table );

    Task<bool> RemoveTableAsync( int tableId );

    Task<bool> UpdateTableAsync( UpdateTableDTO updateTableDTO );

    Task<IEnumerable<Table>> GetAllTablesAsync(CancellationToken cancellationToken);

    Task<bool> IsDuplicateName( string tableName );
}