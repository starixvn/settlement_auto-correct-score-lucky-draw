using Core.Constants;
using Core.Models;
using CorrectScoreLuckyDraw.Models;
using CorrectScoreLuckyDraw.Services;
using NSubstitute;

namespace CorrectScoreLuckyDraw.UnitTests;

public class AutoNotificationControllerTests
{
    private readonly NotificationService notificationService;

    private readonly SiteMessageService siteMessageService;

    private readonly AutoNotificationController service;

    public AutoNotificationControllerTests()
    {
        notificationService = Substitute.For<NotificationService>();
        siteMessageService = Substitute.For<SiteMessageService>();
        service = new AutoNotificationControllerImp(notificationService, siteMessageService);
    }

    [Fact]
    public async Task Process_IsNotExistedNotification_ReturnsNotCallAnyMethod()
    {
        notificationService.GetNotification().Returns([]);
        await service.Process();

        await siteMessageService.DidNotReceive().SiteIBCIndo(Arg.Any<Ticket>());
        await siteMessageService.DidNotReceive().SiteNotIBCIndo(Arg.Any<Ticket>());
        await notificationService.DidNotReceive().UpdateNotification(Arg.Any<string>());
    }

    [Fact]
    public async Task Process_IsExistedNotificationAndErrorGetMessageBySite_ReturnsNotCallAnyMethod()
    {
        var notification = new List<Ticket>
        {
            new()
            {
                MatchId = 1,
                Site = "ABC"
            }
        };

        notificationService.GetNotification().Returns(notification);
        siteMessageService.SiteNotIBCIndo(Arg.Any<Ticket>()).Returns(new Result { ErrorCode = 1 });
        await service.Process();

        await siteMessageService.DidNotReceive().SiteIBCIndo(Arg.Any<Ticket>());
        await notificationService.DidNotReceive().UpdateNotification(Arg.Any<string>());
    }

    [Fact]
    public async Task Process_IsExistedNotificationAndGetMessageBySite_CallUpdateNotificationMethod()
    {
        var notification = new List<Ticket>
        {
            new()
            {
                MatchId = 1,
                Site = Site.IBCIndo,
                DrawId = 1
            }
        };

        notificationService.GetNotification().Returns(notification);
        siteMessageService.SiteIBCIndo(Arg.Any<Ticket>()).Returns(new Result { ErrorCode = 0 });
        await service.Process();

        await notificationService.Received().UpdateNotification(Arg.Any<string>());
    }
}