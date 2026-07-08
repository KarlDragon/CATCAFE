using BE.Services.Interfaces;
using Microsoft.Extensions.Hosting;

namespace BE.Services.Implementations;

public class RegistrationFilterInitializationHostedService : IHostedService
{
    private readonly IRegistrationFilterService _registrationFilterService;

    public RegistrationFilterInitializationHostedService(IRegistrationFilterService registrationFilterService)
    {
        _registrationFilterService = registrationFilterService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _registrationFilterService.InitializeAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
