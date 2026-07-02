using BE.DTOs;

namespace BE.Services.Interfaces;

public interface IAuthService
{
    Task<bool> Register(RegisterDTO registerDTO);
    Task<AuthResponseDTO> Login(LoginDTO loginDTO);
    Task<string?> Refresh (RefreshDTO refreshDTO);
    Task<bool> Logout(RefreshDTO refreshDTO);
}
