namespace BE.Services.Implementations;
using BE.Services.Interfaces;
using StackExchange.Redis;

public class RegistrationFilterService : IRegistrationFilterService
{
    private readonly IDatabase _redisDatabase;
    private readonly SemaphoreSlim _initializationLock = new(1, 1);
    private bool _initialized;
    private const string EmailBloomFilterKey = "bf:users:emails";
    private const string UsernameBloomFilterKey = "bf:users:usernames";

    public RegistrationFilterService(IConnectionMultiplexer redis)
    {
        try
        {
            _redisDatabase = redis.GetDatabase();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to connect to Redis. Please ensure that the Redis server is running and accessible.", ex);
        }
    }

    public async Task InitializeAsync()
    {
        if (_initialized)
        {
            return;
        }

        await _initializationLock.WaitAsync();
        try
        {
            if (_initialized)
            {
                return;
            }

            await EnsureBloomFilterAsync(EmailBloomFilterKey);
            await EnsureBloomFilterAsync(UsernameBloomFilterKey);
            _initialized = true;
        }
        finally
        {
            _initializationLock.Release();
        }
    }

    public async Task<bool> IsEmailRegistered(string email)
    {
        await InitializeAsync();
        var result = await _redisDatabase.ExecuteAsync("BF.EXISTS", EmailBloomFilterKey, email);
        return result.ToString() == "1";
    }

    public async Task<bool> IsUsernameRegistered(string username)
    {
        await InitializeAsync();
        var result = await _redisDatabase.ExecuteAsync("BF.EXISTS", UsernameBloomFilterKey, username);
        return result.ToString() == "1";
    }

    public async Task AddEmailToBloomFilter(string email)
    {
        await InitializeAsync();
        await _redisDatabase.ExecuteAsync("BF.ADD", EmailBloomFilterKey, email);
    }

    public async Task AddUsernameToBloomFilter(string username)
    {
        await InitializeAsync();
        await _redisDatabase.ExecuteAsync("BF.ADD", UsernameBloomFilterKey, username);
    }

    private async Task EnsureBloomFilterAsync(string key)
    {
        var exists = await _redisDatabase.KeyExistsAsync(key);
        if (!exists)
        {
            await _redisDatabase.ExecuteAsync("BF.RESERVE", key, 0.01, 1000000);
        }
    }
}