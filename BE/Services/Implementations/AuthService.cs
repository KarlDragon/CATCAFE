namespace BE.Services.Implementations;
using FluentValidation;
using BE.Repositories.Interfaces;
using BE.Services.Interfaces;
using BE.Models;
using BE.DTOs;

public class AuthService : IAuthService
{
    private readonly RegisterValidator _requestValidation;
    private readonly LoginValidator _loginValidation;
    private readonly IRegistrationFilterService _bloomFilter;
    private readonly IAuthRepository _authRepository;

    public AuthService(RegisterValidator registerValidation, 
                        LoginValidator loginValidation, 
                        IRegistrationFilterService bloomFilter, 
                        IAuthRepository authRepository)
    {
        _requestValidation = registerValidation;
        _loginValidation = loginValidation;
        _bloomFilter = bloomFilter;
        _authRepository = authRepository;
    }

    public async Task<RegisterDTO> Register(RegisterDTO registerDTO)
    {
        var validationResult = await _requestValidation.ValidateAsync(registerDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        // Check Bloom filter for email and username
        if (await _bloomFilter.IsEmailRegistered(registerDTO.Email))
        {
            throw new Exception("Email already exists");
        }
        if (await _bloomFilter.IsUsernameRegistered(registerDTO.Username))
        {
            throw new Exception("Username already exists");
        }
        // Hash the password
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);
        
        var user = new Users
        {
            Username = registerDTO.Username,
            Email = registerDTO.Email,
            PasswordHash = hashedPassword,
            Role = registerDTO.Role,
            Name = registerDTO.Name
        };
        // Add email and username to Bloom filter
        await _bloomFilter.AddEmailToBloomFilter(registerDTO.Email);
        await _bloomFilter.AddUsernameToBloomFilter(registerDTO.Username);

        await _authRepository.RegisterAsync(user);
        return registerDTO;
    }

    public async Task<LoginDTO> Login(LoginDTO loginDTO)
    {
        var validationResult = await _loginValidation.ValidateAsync(loginDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        var user = await _authRepository.GetUserByIdentifierAsync(loginDTO.EmailOrUsername);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
        {
            throw new Exception("Invalid password");
        }
        return loginDTO;
    }
}
