namespace BE.Repositories.Interfaces;
using BE.Models;
public interface ITableRepository
{
    // Add more tables based on the amount created previously.
    // Example: current table count + tableAmount to add.
    Task<bool> CreateTablesAsync( int tableAmount );

    Task<bool> RemoveTableAsync( int tableId );

    Task<bool> BookTableAsync ( Table table );

    // Clear booking informations ( UserId + BookedTime)
    Task<bool> ClearTable( int tableId );

    Task<IEnumerable<Table>> GetAllTablesAsync();

    Task<IEnumerable<Table>> GetUserTablesAsync( int userId );

    Task<Table?> GetTableByIdAsync( int tableId );

}