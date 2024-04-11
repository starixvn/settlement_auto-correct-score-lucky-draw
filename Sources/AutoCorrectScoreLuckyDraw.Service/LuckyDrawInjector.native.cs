using AutoMapper;
using CorrectScoreLuckyDraw.Services;
using Fanex.Caching;
using Fanex.Caching.Redis;
using Fanex.Data.Repository;
using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
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
using SharpRaven;

namespace AutoCorrectScoreLuckyDraw.Service;

public static class LuckyDrawInjector
{
    public static void InitializeServices(this IServiceCollection service, IConfiguration configuration)
    {
        ConfigureAutoMapper(service);
        service.AddTransient<IMaintenanceService, MaintenanceServiceImp>();
        service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        service.AddSingleton<AutoMatchController, AutoMatchControllerImp>();
        service.AddSingleton<MatchService, MatchServiceImp>();
        service.AddSingleton<MemberService, MemberServiceImp>();
        service.AddSingleton<SettleLogService, SettleLogServiceImp>();
        service.AddSingleton<AutoNotificationController, AutoNotificationControllerImp>();
        service.AddSingleton<RefDataService, RefDataServiceImp>();
        service.AddSingleton<BatchMessageService, BatchMessageServiceImp>();
        service.AddSingleton<NotificationService, NotificationServiceImp>();
        service.AddSingleton<MessageAnnouncementService, MessageAnnouncementServiceImp>();
        service.AddSingleton<SiteMessageService, SiteMessageServiceImp>();

        service.AddSingleton<XmlConvertService, XmlConvertServiceImp>();
        service.AddSingleton<IDynamicRepository, DynamicRepository>();
        service.AddSingleton(new UMClientConfiguration().Initialize(configuration));
        service.AddSingleton<IUMCacheService, UMCacheService>();
        service.AddSingleton<IUMRestClient, UMRestClient>();
        service.AddSingleton<ONEbook.UM.Services.IMaintenanceService, ONEbook.UM.Services.MaintenanceService>();
        service.AddMemoryCache();
        service.ConfigureRedis(configuration);
        RefDataTypeContainer
            .ConnectTo(new Uri(configuration.GetSection("Fanex.RemotingRef.Service.Url").Value))
            .FromApplication("Settlement");
        service.AddSingleton<IRemotingRefService, RemotingRefService>();
        StartUmSetting(configuration);
        StartLogSetting(configuration);
    }

    private static void ConfigureRedis(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddSingleton<ICacheService, CacheService>();
        service.AddRedisCache(configuration.GetSection("Fanex.Caching.Redis"));
    }

    private static void StartUmSetting(IConfiguration configuration)
    {
        var umConfig = new UMClientConfiguration().Initialize(configuration);

        UMClientManager
            .UseConfig(umConfig)
            .UseCacheService(new UMCacheService(new CacheService()))
            .UseRestClient(new UMRestClient(umConfig));
    }

    private static void StartLogSetting(IConfiguration configuration)
    {
        var logEngine = new List<ILogEngine>();

        var sentryEngineOptions = new SentryEngineOptions
        {
            Dsn = new Dsn(configuration.GetSection("SentryToken").Value)
        };

        logEngine.Add(new SentryLogging(sentryEngineOptions));

        LogManager.SetDefaultLogCategory(configuration.GetSection("SiteCategory").Value).Use(logEngine);
    }

    private static void ConfigureAutoMapper(IServiceCollection service)
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            //mc.AddProfile(typeof(ScaleOutMappingProfile));
        });

        var mapper = mapperConfig.CreateMapper();
        service.AddSingleton(mapper);
    }
}