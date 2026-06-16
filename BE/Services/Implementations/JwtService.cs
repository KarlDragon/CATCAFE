namespace BE.Services.Implementations;
using BE.Models;
using BE.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BE.Repositories.Interfaces;
public class JwtService : IJwtService
{
    private readonly IConfiguration _config;
    private readonly IAuthRepository _authRepository;
    public JwtService( IConfiguration configuration, IAuthRepository authRepository)
    {
        _config = configuration;
        _authRepository = authRepository;
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

    public string GernerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public bool ValidateRefreshToken(int userID)
    {
        var token = _authRepository.GetUserByIdentifierAsync(userID.ToString());
        if (token == null ) return false;
        return true;
    }
}