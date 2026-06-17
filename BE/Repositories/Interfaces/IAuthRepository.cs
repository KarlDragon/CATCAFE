namespace BE.Repositories.Interfaces;
using BE.Models;

public interface IAuthRepository
{
    // Register a new user and return boolean indicating success or failure
    Task<bool> RegisterAsync(Users user);

    // Login a user by email or username and return the user object if found 
    Task<Users?> GetUserByIdentifierAsync(string identifier);

    public Task<Users?> GetUserById(int id);

    // Save a refresh token for a user and return a boolean indicating success
    Task<bool> DeleteRefreshTokenAsync(string token);

    // Create a new refresh token for a user and return the created token object
    Task<RefreshToken> CreateRefreshTokenAsync(RefreshToken token);

    // Get a refresh token and return the associated user if valid
    Task<RefreshToken?> GetRefreshTokenAsync(int userID);


}