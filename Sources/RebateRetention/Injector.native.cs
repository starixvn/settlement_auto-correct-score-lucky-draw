using Microsoft.Extensions.DependencyInjection;
using RebateRetention.Services;
using RebateRetention.Services.ProgramMessage;

namespace RebateRetention;

public static class Injector
{
    public static IServiceCollection AddRebateRetention(this IServiceCollection services)
    {
        services.AddSingleton<AutoRebateRetentionController, AutoRebateRetentionControllerImp>();
        services.AddSingleton<SabaWalletService, SabaWalletServiceImp>();
        services.AddSingleton<SabaProgramStatusService, SabaProgramStatusServiceImp>();
        services.AddSingleton<GenerateMessageService, GenerateMessageServiceImp>();
        services.AddSingleton<MessageTemplateProviderFactory, MessageTemplateProviderFactoryImp>();
        services.AddSingleton<IMessageTemplateProvider, DefaultTemplateProvider>();
        services.AddSingleton<IMessageTemplateProvider, ProgramId3TemplateProvider>();
        return services;
    }
}