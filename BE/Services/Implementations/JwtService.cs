namespace BE.Services.Implementations;
using BE.Models;
using BE.Services.Interfaces;
using Microsoft.Extensions.Configuration;
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
    private readonly IAuthRepository _authRepository;
    private readonly ILogger _logger;
    public JwtService( IConfiguration configuration, IAuthRepository authRepository, ILogger logger)
    {
        _config = configuration;
        _authRepository = authRepository;
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
            new Claim(ClaimTypes.Role, users.Role)
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
        var refreshToken = await _authRepository.GetRefreshTokenAsync(userID);

        if ( refreshToken == null) return false;
        if ( refreshToken.Expires <= DateTime.UtcNow) return false;

        
        // get both token byte
        byte[] refreshTokenByte = Convert.FromHexString(refreshToken.Token);
        byte[] clientRefreshTokenbyte = SHA256.HashData(Encoding.UTF8.GetBytes(rawRefreshToken));

        if (!CryptographicOperations.FixedTimeEquals(refreshTokenByte, clientRefreshTokenbyte)) return false;

        return true;
    }

    public async Task<bool> ValidateTempTokenAsync( string token )
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        // Get secret key, same thing in jwt Service :V
        var jwtSettings = _config.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"];
        var validateIssuer = jwtSettings["Issuer"];
        var validateAudience = jwtSettings["Audience"];

        if (secretKey == null || validateIssuer == null || validateAudience == null)
        {
            throw new Exception("CHECK THE JWTSETTINGS!!!!");
        }

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidIssuer = validateIssuer,
            ValidAudience = validateAudience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try{
            var claimsPrincipal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            var jwtToken = validatedToken as JwtSecurityToken;
            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            var idValue = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(idValue, out int userId))
            {
                _logger.LogError(" Seem like can't parse userId from token, re-check it");
                return false;
            }

            var user = await _authRepository.GetUserById(userId);
            if (user == null)
            {
                return false;
            }

            return true;
        }
        catch ( SecurityTokenException )
        {
            _logger.LogWarning("Invalid token");
            return false;
        }
    }
}