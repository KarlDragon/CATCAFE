namespace BE.Repositories.Interfaces;
using BE.Models;
public interface IAuthRepository
{
    // Register a new user and return boolean indicating success or failure
    Task RegisterAsync(Users user);

    // Login a user by email or username and return the user object if found 
    Task<Users?> GetUserByIdentifierAsync(string identifier);

    public Task<Users?> GetUserById(int id);
}