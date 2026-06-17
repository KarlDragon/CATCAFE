namespace BE.Services.Interfaces;

using BE.DTOs;
using BE.Models;
public interface IJwtService
{
    public string GenerateToken(Users users);
    public string GenerateRefreshToken();
    public Task<bool> ValidateRefreshTokenAsync(int userID, string refreshTokenDTO);
    public Task<bool> ValidateTempTokenAsync( string token);
}

