namespace BE.Services.Interfaces;
using BE.Models;
public interface IJwtService
{
    public string GenerateToken(Users users);
    public string GernerateRefreshToken();
    public bool ValidateRefreshToken(int userID);
}

