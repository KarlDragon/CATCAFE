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
        var refreshToken = await GetRefreshTokenAsync(userId);
        if (refreshToken == null) return false;

        _context.RefreshTokens.Remove(refreshToken);
        await _context.SaveChangesAsync();
        return true;
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