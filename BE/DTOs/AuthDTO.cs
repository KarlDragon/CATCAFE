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
}