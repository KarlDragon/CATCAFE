namespace BE.Repositories.Interfaces;
using BE.Models;

public interface IAuthRepository
{
    Task<Users> RegisterAsync(Users user);

    Task<Users> LoginAsync(string email, string passwordhash);

    Task<Users> LogoutAsync();


}