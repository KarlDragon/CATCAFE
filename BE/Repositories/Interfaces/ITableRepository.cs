namespace BE.Repositories.Interfaces;
using BE.Models;
public interface ITableRepository
{
    // Add more tables based on the amount created previously.
    // Example: current table count + tableAmount to add.
    Task<bool> CreateTablesAsync( int tableAmount );

    Task<bool> RemoveTableAsync( int tableId );

    // Change UserId + BookedTime where UserId is Null
    Task<bool> BookTableAsync( Table table );

    // Clear booking informations ( UserId + BookedTime)
    Task<bool> ClearTableAsync( int tableId );

    Task<IEnumerable<Table>> GetAllTablesAsync(CancellationToken cancellationToken);

    Task<IEnumerable<Table>> GetUserTablesAsync( int userId, CancellationToken cancellationToken );

    Task<Table?> GetTableByIdAsync( int tableId );

}