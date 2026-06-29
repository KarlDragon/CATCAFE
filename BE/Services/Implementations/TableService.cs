namespace BE.Services.Implementations;
using BE.Services.Interfaces;
using BE.Models;
using BE.DTOs;
using BE.Repositories.Interfaces;
using System.Data;
using System.Threading.Tasks;

public class TableService : ITableService
{
    private readonly ITableRepository _tableRepository;
    private readonly ILogger _logger;

    public TableService( ITableRepository tableRepository, ILogger logger)
    {
        _tableRepository = tableRepository;
        _logger = logger;
    }

    public async Task<bool> CreateTableAsync( Table table )
    {
        table.TableName = table.TableName.Trim();
        var duplicated = await _tableRepository.IsDuplicateName(table.TableName);
        if (duplicated)
        {
            throw new DuplicateNameException(" Trùng tên bàn ");
        }
        return await _tableRepository.CreateTableAsync(table);
    }

    public async Task RemoveTableAsync( int tableId )
    {
        var result = await _tableRepository.RemoveTableAsync(tableId);
        if (!result)
        {
            throw new Exception($"Can't remove table {tableId}");
        }
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