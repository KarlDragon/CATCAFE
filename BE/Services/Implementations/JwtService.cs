namespace BE.Services.Implementations;
using BE.Models;
using BE.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BE.Repositories.Interfaces;
using System.Threading.Tasks;
using System.Security.Cryptography;

public class JwtService : IJwtService
{
    private readonly IConfiguration _config;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ILogger<JwtService> _logger;
    public JwtService(  IConfiguration configuration, 
                        IRefreshTokenRepository refreshTokenRepository, 
                        ILogger<JwtService> logger)
    {
        _config = configuration;
        _refreshTokenRepository = refreshTokenRepository;
        _logger = logger;
    }

    public string GenerateToken(Users users)
    {
        var jwtSettings = _config.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        if (secretKey == null)
        {
            throw new Exception("CHECK THE SECRETKEY IN JWTSETTINGS!!!!");
        }
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials( key, SecurityAlgorithms.HmacSha256 );

        var Claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, users.Id.ToString()),
            new Claim(ClaimTypes.Name, users.Username),
            new Claim(ClaimTypes.Role, users.Role.ToString())
        };
        
        var durationMinutes = int.TryParse(jwtSettings["DurationInMinutes"], out var minutes)
            ? minutes
            : throw new Exception("CHECK THE DurationInMinutes IN JWTSETTINGS!!!!");

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: Claims,
            expires: DateTime.UtcNow.AddMinutes(durationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var bytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        var refreshToken = Convert.ToBase64String(bytes).Replace('+', '-').Replace('/','_').TrimEnd('=');
        return refreshToken;
    }

    public async Task<bool> ValidateRefreshTokenAsync(int userID, string rawRefreshToken)
    {
        var refreshToken = await _refreshTokenRepository.GetRefreshTokenAsync(userID);

        if ( refreshToken == null)
        {
            _logger.LogInformation(" There's no refreshtoken with this user: {user}", userID);
            return false;
        }
        if ( refreshToken.Expires <= DateTime.UtcNow)
        { 
            _logger.LogInformation(" This users' {user} token is expired", userID);
            return false;
        }

        
        // get both token byte
        byte[] refreshTokenByte = Convert.FromHexString(refreshToken.Token);
        byte[] clientRefreshTokenbyte = SHA256.HashData(Encoding.UTF8.GetBytes(rawRefreshToken));

        if (!CryptographicOperations.FixedTimeEquals(refreshTokenByte, clientRefreshTokenbyte))
        { 
            _logger.LogWarning("UserId: {user} is right but wrong refresh token, pls check this request!", userID);
            return false;
        }

        return true;
    }
}