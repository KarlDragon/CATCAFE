namespace BE.Repositories.Implementations;
using BE.Repositories.Interfaces;
using BE.Models;
using Microsoft.EntityFrameworkCore;

public class CatRepository : ICatRepository
{
    private readonly AppDbContext _context;

    public CatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateCatAsync( Cat cat )
    {
        _context.Cats.Add(cat);
        var affectedRow = await _context.SaveChangesAsync();
        return affectedRow > 0;
    }

    public async Task<bool> RemoveCatAsync( int catId )
    {
        var cat = await GetCatByIdAsync(catId);
        if (cat != null)
        {
            _context.Cats.Remove(cat);
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<IEnumerable<Cat>> GetAllCatsAsync( CancellationToken cancellationToken )
    {
        return await _context.Cats.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<Cat?> GetCatByIdAsync( int catId )
    {
        return await _context.Cats.FirstOrDefaultAsync(c => c.CatID == catId);
    }

}