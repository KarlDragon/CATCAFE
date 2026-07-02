namespace BE.Repositories.Implementations;
using BE.Repositories.Interfaces;
using BE.Models;
using Microsoft.EntityFrameworkCore;
public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;

    public AuthRepository( AppDbContext context)
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
}