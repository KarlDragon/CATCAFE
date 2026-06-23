namespace BE.Services.Implementations;
using BE.Repositories.Interfaces;
using BE.Services.Interfaces;
using BE.Models;
using BE.DTOs;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
public class AuthService : IAuthService
{
    private readonly IRegistrationFilterService _bloomFilter;
    private readonly IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IJwtService _iJwtService;
    private readonly ILogger<AuthService> _logger;
    public AuthService(IRegistrationFilterService bloomFilter, 
                        IUserRepository userRepository,
                        IRefreshTokenRepository refreshTokenRepository,
                        IJwtService jwtService,
                        ILogger<AuthService> logger
                        )
    {
        _bloomFilter = bloomFilter;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _iJwtService = jwtService;
        _logger = logger;
    }

    public async Task<bool> Register(RegisterDTO registerDTO)
    {
        // Check Bloom filter for email and username
        if (await _bloomFilter.IsEmailRegistered(registerDTO.Email))
        {
            var existingUser = await _userRepository.GetUserByIdentifierAsync(registerDTO.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Email already registered.");
            }
        }
        if (await _bloomFilter.IsUsernameRegistered(registerDTO.Username))
        {
            var existingUser = await _userRepository.GetUserByIdentifierAsync(registerDTO.Username);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Username already taken.");
            }
        }

        // Hash the password
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDTO.Password);
        
        var user = new Users
        {
            Username = registerDTO.Username,
            Email = registerDTO.Email,
            PasswordHash = hashedPassword,
            Role = Enum.Parse<Users.UserRole>(registerDTO.Role, ignoreCase: true),
            Name = registerDTO.Name
        };

        bool isRegistered = await _userRepository.RegisterAsync(user);
        if (!isRegistered){
            return false;
        }    
        
        await _bloomFilter.AddEmailToBloomFilter(registerDTO.Email);
        await _bloomFilter.AddUsernameToBloomFilter(registerDTO.Username);
        return true;
    }

    public async Task<AuthResponseDTO> Login(LoginDTO loginDTO)
    {
        var user = await _userRepository.GetUserByIdentifierAsync(loginDTO.EmailOrUsername);
        if (user == null)
        {
            _logger.LogInformation( "User not found: {userName}", loginDTO.EmailOrUsername);
            throw new UnauthorizedAccessException();
        }
        if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
        {
            _logger.LogInformation( " Wrong password: {userName}", loginDTO.EmailOrUsername );
            throw new UnauthorizedAccessException("Invalid password");
        }

        // Delete old refresh token when login
        await _refreshTokenRepository.DeleteRefreshTokenAsync(user.Id);
        //temp jwt token and refreshToken will be created anyways
        //refresh token is only generated if user log out or it's expired
        var token = _iJwtService.GenerateToken(user);
        var refreshToken = _iJwtService.GenerateRefreshToken();

        // hash and save refreshToken
        var hashRefreshToken = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(refreshToken)));
        var storedRefreshToken = new RefreshToken
        {
            Token = hashRefreshToken,
            Expires = DateTime.UtcNow.AddDays(30),
            UserID = user.Id
        };
        await _refreshTokenRepository.CreateRefreshTokenAsync(storedRefreshToken);

        return new AuthResponseDTO{
            Token = token,
            RefreshToken = refreshToken,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role.ToString(),
            Name = user.Name
        };
    }

    public async Task<string?> Refresh (RefreshDTO refreshDTO)
    {
        if (await _iJwtService.ValidateRefreshTokenAsync(refreshDTO.UserId, refreshDTO.RefreshToken))
        {
            var user = await _userRepository.GetUserById(refreshDTO.UserId);
            if ( user == null)
            {
                _logger.LogWarning(" User don't exist ");
                return null;
            }
            var token = _iJwtService.GenerateToken(user);
            return token;
        }
        else
        {
            _logger.LogWarning(" RefreshToken invalid");
            return null;
        }

    }
}
