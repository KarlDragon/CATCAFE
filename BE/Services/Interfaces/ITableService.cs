namespace BE.Services.Interfaces;
using BE.Models;
using BE.DTOs;
public interface ITableService
{
    public Task<bool> CreateTableAsync( CreateTableDTO createTableDTO );
    public Task RemoveTableAsync( int tableId );
    public Task<bool> UpdateTableAsync( UpdateTableDTO updateTableDTO );
    public Task<IEnumerable<Table>> GetAllTablesAsync( CancellationToken cancellationToken );

}
