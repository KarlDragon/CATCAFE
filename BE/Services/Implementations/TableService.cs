namespace BE.Services.Implementations;
using BE.Services.Interfaces;
using BE.Models;
using BE.DTOs;
using BE.Repositories.Interfaces;
using BE.Repositories.Implementations;
using System.Data;

public class TableService : ITableService
{
    private readonly TableRepository _tableRepository;
    private readonly ILogger _logger;

    public TableService( TableRepository tableRepository, ILogger logger)
    {
        _tableRepository = tableRepository;
        _logger = logger;
    }

    public async Task<bool> CreateTableAsync( Table table )
    {
        var duplicated = await _tableRepository.IsDuplicateName(table.TableName);
        if (duplicated)
        {
            throw new DuplicateNameException(" Trùng tên bàn ");
        }

        var newTable = new Table
        {
            TableName = table.TableName.Trim(),
            SeatAmount = table.SeatAmount
        };
        return true;
    }

    public bool RemoveTable( int tableId)
    {
        return true;
    }
    public bool UpdateTable( UpdateTableDTO updateTableDTO)
    {
        return true;
    }
    public Task<IEnumerable<Table>> GetAllTables( CancellationToken cancellationToken )
    {
        var AllTable = _tableRepository.GetAllTablesAsync(cancellationToken);
        return AllTable;
    }
}