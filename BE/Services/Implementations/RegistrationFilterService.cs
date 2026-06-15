namespace BE.Services.Implementations;
using BE.Services.Interfaces;
using StackExchange.Redis;
public class RegistrationFilterService : IRegistrationFilterService
{
    private readonly IDatabase _redisDatabase;
    private const string EmailBloomFilterKey = "bf:users:emails";
    private const string UsernameBloomFilterKey = "bf:users:usernames";
    public RegistrationFilterService(IConnectionMultiplexer redis)
    {
        _redisDatabase = redis.GetDatabase();

        if (!_redisDatabase.KeyExists(EmailBloomFilterKey))
        {
            _redisDatabase.Execute("BF.RESERVE", EmailBloomFilterKey, 0.01, 1000000);
        }
        if (!_redisDatabase.KeyExists(UsernameBloomFilterKey))
        {
            _redisDatabase.Execute("BF.RESERVE", UsernameBloomFilterKey, 0.01, 1000000);
        }
    }
    public Task<bool> IsEmailRegistered(string email)
    {
        return Task.FromResult(_redisDatabase.ExecuteAsync("BF.EXISTS", EmailBloomFilterKey, email).ToString() == "1");
    }

    public Task<bool> IsUsernameRegistered(string username)
    {
        return Task.FromResult(_redisDatabase.ExecuteAsync("BF.EXISTS", UsernameBloomFilterKey, username).ToString() == "1");
    }

    public Task AddEmailToBloomFilter(string email)
    {
        _redisDatabase.ExecuteAsync("BF.ADD", EmailBloomFilterKey, email);
        return Task.CompletedTask;
    }

    public Task AddUsernameToBloomFilter(string username)
    {
        _redisDatabase.ExecuteAsync("BF.ADD", UsernameBloomFilterKey, username);
        return Task.CompletedTask;
    }
}