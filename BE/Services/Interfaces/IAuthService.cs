using BE.DTOs;

namespace BE.Services.Interfaces;

public interface IAuthService
{
    Task<RegisterDTO> Register(RegisterDTO registerDTO);
    Task<LoginDTO> Login(LoginDTO loginDTO);
}
