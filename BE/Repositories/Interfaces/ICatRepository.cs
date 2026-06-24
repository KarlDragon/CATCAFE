namespace BE.Repositories.Interfaces;
using BE.Models;
public interface ICatRepository
{
    Task<bool> CreateCatAsync( Cat cat );

    Task<bool> RemoveCatAsync( int catId );

    Task<IEnumerable<Cat>> GetAllCatsAsync(CancellationToken cancellationToken);

    Task<Cat?> GetCatByIdAsync( int catId );
}