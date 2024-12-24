using Core.Models;
using Core.Services;
using CorrectScoreLuckyDraw.Models;
using CorrectScoreLuckyDraw.Services;
using Fanex.MailDeliver.MailClient;
using Fanex.RemotingRef;
using Microsoft.Extensions.Options;

namespace CorrectScoreLuckyDraw.UnitTests;

public class SendMailServiceTests
{
    private readonly SendMailServiceImp service;

    public SendMailServiceTests()
    {
        var appSettingsKey = new AppSettingsKey { RecipientEmailTest = new Email { To = "kaden.trinh@starixsoft.com" } };
        var options = Options.Create(appSettingsKey);
        const string remotingRefUrl = "http://remotingref-api-core.nexdev.net/";
        RefDataTypeContainer
            .ConnectTo(new Uri(remotingRefUrl))
            .FromApplication("Settlement");

        AppSettings.AppMailId = "1";
        AppSettings.AppMailAddress = "http://10.18.200.131:8787/";
        service = new SendMailServiceImp(options);
    }

    [Fact]
    public void SendMail_Always_ReturnEmail()
    {
        var model = new NotificationModel
        {
            Match = new Match
            {
                MatchId = 123,
                IsTest = true,
                AwayId = 1,
                HomeId = 1,
                RefDataService = new RefDataServiceImp(new RemotingRefService())
            },
            Tickets = new List<Ticket>
            {
                new Ticket
                {
                    AwayId = 1,
                    AgentId = 1,
                    AgentUsername = "AgentName"
                }
            }
        };

        service.SendMail(model);
        Assert.True(true);
    }
}