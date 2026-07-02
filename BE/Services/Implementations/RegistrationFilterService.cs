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
        try{
            _redisDatabase = redis.GetDatabase();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to connect to Redis. Please ensure that the Redis server is running and accessible.", ex);
        }

        if (!_redisDatabase.KeyExists(EmailBloomFilterKey))
        {
            _redisDatabase.Execute("BF.RESERVE", EmailBloomFilterKey, 0.01, 1000000);
        }
        if (!_redisDatabase.KeyExists(UsernameBloomFilterKey))
        {
            _redisDatabase.Execute("BF.RESERVE", UsernameBloomFilterKey, 0.01, 1000000);
        }
    }
    public async Task<bool> IsEmailRegistered(string email)
    {
        var result = await _redisDatabase.ExecuteAsync("BF.EXISTS", EmailBloomFilterKey, email);
        return result.ToString() == "1";
    }

    public async Task<bool> IsUsernameRegistered(string username)
    {
        var result = await _redisDatabase.ExecuteAsync("BF.EXISTS", UsernameBloomFilterKey, username);
        return result.ToString() == "1";
    }

    public async Task AddEmailToBloomFilter(string email)
    {
        await _redisDatabase.ExecuteAsync("BF.ADD", EmailBloomFilterKey, email);
    }

    public async Task AddUsernameToBloomFilter(string username)
    {
        await _redisDatabase.ExecuteAsync("BF.ADD", UsernameBloomFilterKey, username);
    }
}