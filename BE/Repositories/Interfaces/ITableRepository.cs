namespace BE.Repositories.Interfaces;

public interface ITableRepository
{
    // Add more tables based on the amount created previously.
    // Example: current table count + tableAmount to add.
    Task<bool> CreateTablesAsync( int tableAmount );

    Task<bool> RemoveTableAsync( int tableId );

    Task<bool> BookTableAsync ( int tableId, int userId, DateTime bookedTime );

    // Clear booking informations
    Task<bool> ClearTable( int tableId );

}