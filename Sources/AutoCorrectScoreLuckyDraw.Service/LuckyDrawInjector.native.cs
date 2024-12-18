using Core.Services;
using CorrectScoreLuckyDraw.Services;
using Fanex.Data.Repository;
using Fanex.Logging.Sentry.RavenClient;
using Fanex.MailDeliver.MailClient;
using Fanex.RemotingRef;
using Fanex.Settlement.Converter.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ONEbook.UM;
using ONEbook.UM.AspNetCore;
using ONEbook.UM.Configurations;
using ONEbook.UM.Helpers;
using ONEbook.UM.Transport;
using RebateRetention;

namespace AutoCorrectScoreLuckyDraw.Service;

public static class LuckyDrawInjector
{
    public static void InitializeServices(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddFanexLogSentry(configuration);

        service.AddTransient<IMaintenanceService, MaintenanceServiceImp>();
        service.AddSingleton<IMessageService, MessageService>();
        service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        service.AddSingleton<AutoMatchController, AutoMatchControllerImp>();
        service.AddSingleton<MatchService, MatchServiceImp>();
        service.AddSingleton<MemberService, MemberServiceImp>();
        service.AddSingleton<SettleLogService, SettleLogServiceImp>();
        service.AddSingleton<AutoNotificationController, AutoNotificationControllerImp>();
        service.AddSingleton<RefDataService, RefDataServiceImp>();
        service.AddSingleton<NotificationService, NotificationServiceImp>();
        service.AddSingleton<MessageAnnouncementService, MessageAnnouncementServiceImp>();
        service.AddSingleton<SiteMessageService, SiteMessageServiceImp>();
        service.AddSingleton<SendMailService, SendMailServiceImp>();
        service.AddRebateRetention();

        service.AddSingleton<XmlConvertService, XmlConvertServiceImp>();
        service.AddSingleton<IDynamicRepository, DynamicRepository>();
        service.AddSingleton(new UMClientConfiguration().Initialize(configuration));
        service.AddSingleton<IUMCacheService, UMCacheService>();
        service.AddSingleton<IUMRestClient, UMRestClient>();
        service.AddSingleton<ONEbook.UM.Services.IMaintenanceService, ONEbook.UM.Services.MaintenanceService>();
        service.AddMemoryCache();

        RefDataTypeContainer
            .ConnectTo(new Uri(configuration.GetSection("Fanex.RemotingRef.Service.Url").Value))
            .FromApplication("Settlement");

        service.AddSingleton<IRemotingRefService, RemotingRefService>();
        StartUmSetting(configuration);

        AppSettings.AppMailId = configuration.GetSection("MailServer:Id").Value;
        AppSettings.AppMailAddress = configuration.GetSection("MailServer:Address").Value;
    }

    private static void StartUmSetting(IConfiguration configuration)
    {
        var umConfig = new UMClientConfiguration().Initialize(configuration);

        UMClientManager
            .UseConfig(umConfig)
            .UseCacheService(new UMCacheService(new Fanex.Caching.CacheService()))
            .UseRestClient(new UMRestClient(umConfig));
    }
}