namespace BE.Models;

public class Users
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public UserRole Role { get; set; } = UserRole.Customer;
    public string Name { get; set; } = "";

    public enum UserRole
    {
        Owner,
        Staff,
        Customer
    }
}

