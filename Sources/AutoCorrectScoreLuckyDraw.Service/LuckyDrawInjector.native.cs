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
using ONEbook.UM.DependencyInjection;
using RebateRetention;

namespace AutoCorrectScoreLuckyDraw.Service;

public static class LuckyDrawInjector
{
    public static void InitializeServices(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddFanexLogSentry(configuration);
        service.AddOneBookUm(configuration);

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
        service.AddMemoryCache();

        RefDataTypeContainer
            .ConnectTo(new Uri(configuration.GetSection("Fanex.RemotingRef.Service.Url").Value))
            .FromApplication("Settlement");

        service.AddSingleton<IRemotingRefService, RemotingRefService>();

        AppSettings.AppMailId = configuration.GetSection("MailServer:Id").Value;
        AppSettings.AppMailAddress = configuration.GetSection("MailServer:Address").Value;
    }
}