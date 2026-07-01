namespace BE.Services.Interfaces;
using BE.Models;
using BE.DTOs;

public interface ICatService
{
    Task<bool> CreateCatAsync( CreateCatDTO createCatDTO );

    Task RemoveCatAsync( int catId );

    Task<bool> UpdateCatAsync( UpdateCatDTO updateCatDTO );

    Task<IEnumerable<Cat>> GetAllCatsAsync(CancellationToken cancellationToken);
}