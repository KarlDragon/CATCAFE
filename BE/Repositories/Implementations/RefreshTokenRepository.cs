namespace BE.Repositories.Implementations;
using BE.Repositories.Interfaces;
using BE.Models;
using Microsoft.EntityFrameworkCore;
public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;

    public RefreshTokenRepository( AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> DeleteRefreshTokenAsync(int userId)
    {
        return await _context.RefreshTokens.Where(rt => rt.UserID == userId).ExecuteDeleteAsync() > 0;
    }

    public async Task<RefreshToken> CreateRefreshTokenAsync(RefreshToken token)
    {
        _context.RefreshTokens.Add(token);
        await _context.SaveChangesAsync();
        return token;
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(int userID)
    {
        return await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserID == userID);
    }
}