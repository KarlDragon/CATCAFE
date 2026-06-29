namespace BE.Services.Implementations;
using BE.Services.Interfaces;
using BE.Models;
using BE.DTOs;
using BE.Repositories.Interfaces;
using System.Threading.Tasks;

public class TableService : ITableService
{
    private readonly ITableRepository _tableRepository;

    public TableService( ITableRepository tableRepository)
    {
        _tableRepository = tableRepository;
}

    public async Task<bool> CreateTableAsync( Table table )
    {
        table.TableName = table.TableName.Trim();
        var duplicated = await _tableRepository.IsDuplicateName(table.TableName);
        if (duplicated)
        {
            throw new Exception(" Trùng tên bàn ");
        }
        return await _tableRepository.CreateTableAsync(table);
    }

    public async Task RemoveTableAsync( int tableId )
    {
        var result = await _tableRepository.RemoveTableAsync(tableId);
        if (!result)
        {
            throw new Exception($"Không tìm thấy bàn {tableId} để xóa.");
        }
    }
    public async Task<bool> UpdateTableAsync( UpdateTableDTO updateTableDTO)
    {
        if (updateTableDTO.TableName != null) updateTableDTO.TableName = updateTableDTO.TableName?.Trim();
        return await _tableRepository.UpdateTableAsync(updateTableDTO);
    }
    public async Task<IEnumerable<Table>> GetAllTablesAsync( CancellationToken cancellationToken )
    {
        return await _tableRepository.GetAllTablesAsync(cancellationToken);
    }
}