namespace BE.Repositories.Interfaces;
using BE.Models;
using BE.DTOs;
public interface ICatRepository
{
    Task<bool> CreateCatAsync( Cat cat );

    Task<bool> RemoveCatAsync( int catId );

    Task<bool> UpdateCatAsync( UpdateCatDTO updateCatDTO );

    Task<IEnumerable<Cat>> GetAllCatsAsync(CancellationToken cancellationToken);

    Task<Cat?> GetCatByIdAsync( int catId );
}