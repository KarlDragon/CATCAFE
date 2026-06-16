namespace BE.Services.Implementations;
using FluentValidation;
using BE.Repositories.Interfaces;
using BE.Services.Interfaces;
using BE.Models;
using BE.DTOs;

public class AuthService : IAuthService
{
    private readonly RegisterValidator _registerValidation;
    private readonly LoginValidator _loginValidation;
    private readonly IRegistrationFilterService _bloomFilter;
    private readonly IAuthRepository _authRepository;
    private readonly IJwtService _iJwtService;
    public AuthService(RegisterValidator registerValidation, 
                        LoginValidator loginValidation, 
                        IRegistrationFilterService bloomFilter, 
                        IAuthRepository authRepository,
                        IJwtService jwtService)
    {
        _registerValidation = registerValidation;
        _loginValidation = loginValidation;
        _bloomFilter = bloomFilter;
        _authRepository = authRepository;
        _iJwtService = jwtService;
    }

    public async Task<bool> Register(RegisterDTO registerDTO)
    {
        // Clean data before validation
        registerDTO.Email = registerDTO.Email.Trim().ToLower();
        registerDTO.Username = registerDTO.Username.Trim().ToLower();

        var validationResult = await _registerValidation.ValidateAsync(registerDTO);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // Check Bloom filter for email and username
        if (await _bloomFilter.IsEmailRegistered(registerDTO.Email))
        {
            var existingUser = await _authRepository.GetUserByIdentifierAsync(registerDTO.Email);
            if (existingUser != null)
            {
                throw new Exception("Email already exists");
            }
        }
        if (await _bloomFilter.IsUsernameRegistered(registerDTO.Username))
        {
            var existingUser = await _authRepository.GetUserByIdentifierAsync(registerDTO.Username);
            if (existingUser != null)
            {
                throw new Exception("Username already exists");
            }
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

        bool isRegistered = await _authRepository.RegisterAsync(user);
        if (!isRegistered){
            await _bloomFilter.AddEmailToBloomFilter(registerDTO.Email);
            await _bloomFilter.AddUsernameToBloomFilter(registerDTO.Username);
        }    
        
        return true;
    }

    public async Task<AuthResponseDTO> Login(LoginDTO loginDTO)
    {
        loginDTO.EmailOrUsername = loginDTO.EmailOrUsername.Trim().ToLower();
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

        string refreshTokenDTO;

        var validatedRefreshToken = await _iJwtService.ValidateRefreshTokenAsync(user.Id);

        if (validatedRefreshToken != null)
        {
            if (loginDTO.RefreshToken != validatedRefreshToken)
            {
                throw new UnauthorizedAccessException("Refresh token mismatch.");
            }
            refreshTokenDTO = validatedRefreshToken; 
        }
        else
        {
            refreshTokenDTO = _iJwtService.GenerateRefreshToken();
            var refreshToken = new RefreshToken
            {
                Token = refreshTokenDTO,
                Expires = DateTime.UtcNow.AddDays(30),
                UserID = user.Id
            };
            await _authRepository.CreateRefreshTokenAsync(refreshToken);
        }

        //temp jwt token will be created anyways
        var token = _iJwtService.GenerateToken(user);
        
        return new AuthResponseDTO{
            Token = token,
            RefreshToken = refreshTokenDTO,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            Name = user.Name
        };
    }
}
