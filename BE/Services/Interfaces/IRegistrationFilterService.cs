namespace BE.Services.Interfaces;
public interface IRegistrationFilterService
{
    Task<bool> IsEmailRegistered(string email);
    Task<bool> IsUsernameRegistered(string username);
    Task AddEmailToBloomFilter(string email);
    Task AddUsernameToBloomFilter(string username);
}