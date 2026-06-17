namespace BE.Repositories.Implementations;
using BE.Repositories.Interfaces;
using BE.Models;
using Microsoft.EntityFrameworkCore;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;

    public AuthRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RegisterAsync(Users user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Users?> GetUserById(int id)
    {
        return await _context.Users.FirstOrDefaultAsync( u => u.Id == id);
    }
    public async Task<Users?> GetUserByIdentifierAsync(string identifier)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == identifier || u.Username == identifier);
    }

    public async Task<bool> DeleteRefreshTokenAsync(int userId)
    {
        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.UserID == userId);
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