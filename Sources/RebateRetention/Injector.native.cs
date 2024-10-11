using Microsoft.Extensions.DependencyInjection;
using RebateRetention.Services;

namespace RebateRetention;

public static class Injector
{
    public static IServiceCollection AddRebateRetention(this IServiceCollection services)
    {
        services.AddSingleton<AutoRebateRetentionController, AutoRebateRetentionControllerImp>();
        services.AddSingleton<SabaWalletService, SabaWalletServiceImp>();
        services.AddSingleton<SabaProgramStatusService, SabaProgramStatusServiceImp>();
        services.AddSingleton<GenerateMessageService, GenerateMessageServiceImp>();

        return services;
    }
}