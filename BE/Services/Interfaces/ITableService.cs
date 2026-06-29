namespace BE.Services.Interfaces;
using BE.Models;
using BE.DTOs;
public interface ITableService
{
    public Task<bool> CreateTableAsync( Table table );
    public bool RemoveTable( int tableId );
    public bool UpdateTable( UpdateTableDTO updateTableDTO );
    public Task<IEnumerable<Table>> GetAllTables( CancellationToken cancellationToken );

}
