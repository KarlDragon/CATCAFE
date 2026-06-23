namespace BE.Repositories.Interfaces;
using BE.Models;
public interface IRefreshTokenRepository
{
    // Save a refresh token for a user and return a boolean indicating success
    Task<bool> DeleteRefreshTokenAsync(int userId);

    // Create a new refresh token for a user and return the created token object
    Task<RefreshToken> CreateRefreshTokenAsync(RefreshToken token);

    // Get a refresh token and return the associated user if valid
    Task<RefreshToken?> GetRefreshTokenAsync(int userID);
}