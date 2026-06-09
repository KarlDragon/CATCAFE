using BE.Models;

namespace BE.Services.Interfaces;

public interface IAuthService
{
    // Method signatures for user registration and login
    Task<Users> Register(string username, string email, string passwordhash, string role, string name);
    Task<Users> Login(string email, string passwordhash);
}
