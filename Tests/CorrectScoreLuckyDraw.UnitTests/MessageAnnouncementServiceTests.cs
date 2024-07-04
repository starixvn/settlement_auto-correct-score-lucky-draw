using CorrectScoreLuckyDraw.Constants;
using CorrectScoreLuckyDraw.Models;
using CorrectScoreLuckyDraw.Services;
using NSubstitute;

namespace CorrectScoreLuckyDraw.UnitTests;

public class MessageAnnouncementServiceTests
{
    private readonly MessageAnnouncementService service;

    public MessageAnnouncementServiceTests()
    {
        var refDataService = Substitute.For<RefDataService>();
        service = new MessageAnnouncementServiceImp(refDataService);
    }

    [Theory]
    [InlineData(Levels.Member, "Dear Valued Customer: Congratulations! You've won a prize of  0 for your bet on \" -vs- \" []. To claim your prize, please contact your upline in 2 weeks. If you are unable to claim your prize from your upline, you can send an email to [support@ibcbet.com]. Don't miss out! <font color='red'>The prize claim period is 2 weeks, ending on 04/07/2024 (GMT+8)</font>. Thank you.")]
    [InlineData(Levels.Agent, "Dear Valued Customer, Congratulations! Your member, , has won the Correct Score Lucky Draw promotion and will receive a prize of  0. The prize claim period is 2 weeks, ending on 04/07/2024 (GMT+8). Thank you.")]
    [InlineData(Levels.Master, "Dear Valued Customer, Congratulations! Your member, , has won the Correct Score Lucky Draw promotion and will receive a prize of  0. The prize claim period is 2 weeks, ending on 04/07/2024 (GMT+8). Thank you.")]
    [InlineData(Levels.Super, "Dear Valued Customer, Congratulations! Your member, , has won the Correct Score Lucky Draw promotion and will receive a prize of  0. The prize claim period is 2 weeks, ending on 04/07/2024 (GMT+8). Thank you.")]
    public void CreateMessageCriteria_IsIBCIndo_ReturnsExpectedMessageValue(string level, string expectedMessage)
    {
        var date = new DateTime(2024, 07, 04, 0, 30, 0, DateTimeKind.Utc);
        var ticket = new Ticket
        {
            Site = Site.IBCIndo,
            ExpiredDate = date
        };

        var actual = service.CreateMessageCriteria(ticket, level);

        Assert.Equal(expectedMessage, actual.ContentEng);
    }

    [Theory]
    [InlineData(Levels.Member, "Dear Valued Customer: Congratulations! You've won a prize of  0 for your bet on \" -vs- \" []. To claim your prize, please contact your upline in 2 weeks. If you are unable to claim your prize from your upline, you can send an email to [support@ibcbet.com]. Don't miss out! <font color='red'>The prize claim period is 2 weeks, ending on 05/07/2024 (GMT+8)</font>. Thank you.")]
    [InlineData(Levels.Agent, "Dear Valued Customer, Congratulations! Your member, , has won the Correct Score Lucky Draw promotion and will receive a prize of  0. The prize claim period is 2 weeks, ending on 05/07/2024 (GMT+8). Thank you.")]
    [InlineData(Levels.Master, "Dear Valued Customer, Congratulations! Your member, , has won the Correct Score Lucky Draw promotion and will receive a prize of  0. The prize claim period is 2 weeks, ending on 05/07/2024 (GMT+8). Thank you.")]
    [InlineData(Levels.Super, "Dear Valued Customer, Congratulations! Your member, , has won the Correct Score Lucky Draw promotion and will receive a prize of  0. The prize claim period is 2 weeks, ending on 05/07/2024 (GMT+8). Thank you.")]
    public void CreateMessageCriteria_IsNotIBCIndo_ReturnsExpectedMessageValue(string level, string expectedMessage)
    {
        var date = new DateTime(2024, 07, 04, 0, 30, 0, DateTimeKind.Utc);
        var ticket = new Ticket
        {
            Site = "Test",
            ExpiredDate = date
        };

        var actual = service.CreateMessageCriteria(ticket, level);

        Assert.Equal(expectedMessage, actual.ContentEng);
    }

    [Theory]
    [InlineData(Levels.Member, 1, "Username")]
    [InlineData(Levels.Agent, 2, "AgentUsername")]
    [InlineData(Levels.Master, 3, "MasterUsername")]
    [InlineData(Levels.Super, 4, "SuperUsername")]
    public void CreateMessageItem_Always_ReturnsExpectedMessageItem(string level, int targetId, string targetName)
    {
        var ticket = new Ticket
        {
            Site = "Test",
            Username = targetName,
            CustId = targetId,

            AgentUsername = targetName,
            AgentId = targetId,

            MasterUsername = targetName,
            MasterId = targetId,

            SuperUsername = targetName,
            SuperId = targetId
        };
        const int messageId = 1;
        var actual = service.CreateMessageItem(ticket, messageId, level);

        var expectedItem = new MessageItem
        {
            MessageId = messageId,
            TargetId = targetId,
            TargetName = targetName
        };

        AssertExtension.AreEqual(expectedItem, actual);
    }
}