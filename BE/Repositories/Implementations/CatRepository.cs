namespace BE.Repositories.Implementations;
using BE.Repositories.Interfaces;
using BE.Models;
using BE.DTOs;
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
        return await _context.Cats.Where(c => c.CatID == catId)
                     .ExecuteUpdateAsync(c => c.SetProperty(c => c.IsActive, false)) > 0;
    }

    public async Task<bool> UpdateCatAsync( UpdateCatDTO updateCatDTO)
    {
        var cat = await _context.Cats.FindAsync( updateCatDTO.CatID );
        if ( cat == null ) return false;

        cat.CatName = updateCatDTO.CatName ?? cat.CatName;
        cat.Breed = updateCatDTO.Breed ?? cat.Breed;
        cat.Status = updateCatDTO.Status ?? cat.Status;
        cat.Description = updateCatDTO.Description ?? cat.Description;
        var affectedRow = await _context.SaveChangesAsync();
        return affectedRow > 0;
    }

    public async Task<IEnumerable<Cat>> GetAllCatsAsync( CancellationToken cancellationToken )
    {
        return await _context.Cats.Where(c => c.IsActive)
                                    .AsNoTracking()
                                    .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsDuplicateName( string catName )
    {
        return await _context.Cats.AnyAsync(c => c.CatName == catName && c.IsActive);
    }
}