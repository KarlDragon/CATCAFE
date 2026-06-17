namespace BE.DTOs;

public class RegisterDTO
{
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public string Role { get; set; } = "";
    public string Name { get; set; } = "";
}

public class LoginDTO
{
    public string EmailOrUsername { get; set; } = "";
    public string Password { get; set; } = "";
    public string RefreshToken { get; set;} = "";
}

public class AuthResponseDTO
{
    public string Token { get; set; } = "";
    public string RefreshToken { get; set;} = "";
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string Role { get; set; } = "";
    public string Name { get; set; } = "";
}

public class RefreshDTO
{
    public int UserId {get; set;}
    public string RefreshToken {get; set;} = "";

}