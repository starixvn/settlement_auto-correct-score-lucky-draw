using Core.Constants;
using NSubstitute;
using RebateRetention.Constants;
using RebateRetention.Models;
using RebateRetention.Services;
using RebateRetention.Services.ProgramMessage;

namespace RebateRetention.UnitTests.Services;

public class GenerateMessageServiceTests
{
    private readonly MessageTemplateProviderFactory templateProviderFactory;
    private readonly GenerateMessageService generateMessageService;

    public GenerateMessageServiceTests()
    {
        templateProviderFactory = Substitute.For<MessageTemplateProviderFactory>();
        generateMessageService = new GenerateMessageServiceImp(templateProviderFactory);
    }

    [Fact]
    public void CreateMessageContent_ShouldReturnMessageWithReplacedProgramName_WhenValidInputsProvided()
    {
        // Arrange
        var ticket = new SabaWalletTicket
        {
            ProgramId = 1,
        };
        const string languageCode = ContentLanguage.English;
        const string level = Levels.Member;
        const string expectedProgramName = "Test Program";
        var expectedTemplate = MessageContent.MemberEng.Replace("@program_name", expectedProgramName);

        var provider = Substitute.For<IMessageTemplateProvider>();
        provider.GetTemplate(level, languageCode, ticket).Returns(expectedTemplate);
        templateProviderFactory.GetProvider(ticket.ProgramId).Returns(provider);

        // Act
        var result = generateMessageService.CreateMessageContent(ticket, languageCode, level);

        // Assert
        Assert.Equal(expectedTemplate, result);
        provider.Received(1).GetTemplate(level, languageCode, ticket);
    }

    [Theory]
    [MemberData(nameof(CreateMessageContentInputData))]
    public void CreateMessageContent_ShouldReturnCorrectTemplateBasedOnInputs(string level, string languageCode, string expectedTemplate)
    {
        // Arrange
        var ticket = new SabaWalletTicket
        {
            ProgramId = 1,
        };
        const string programName = "Test Program";
        var template = expectedTemplate.Replace("@program_name", programName);

        var provider = Substitute.For<IMessageTemplateProvider>();
        provider.GetTemplate(level, languageCode, ticket).Returns(template);
        templateProviderFactory.GetProvider(ticket.ProgramId).Returns(provider);

        // Act
        var result = generateMessageService.CreateMessageContent(ticket, languageCode, level);

        // Assert
        Assert.Equal(template, result);
        provider.Received(1).GetTemplate(level, languageCode, ticket);
    }

    [Fact]
    public void CreateMessageContent_ShouldReturnEmptyString_WhenInvalidLevelProvided()
    {
        // Arrange
        var ticket = new SabaWalletTicket
        {
            ProgramId = 1,
        };
        const string languageCode = ContentLanguage.English;
        const string invalidLevel = "InvalidLevel";
        var provider = Substitute.For<IMessageTemplateProvider>();
        provider.GetTemplate(invalidLevel, languageCode, ticket).Returns(string.Empty);
        templateProviderFactory.GetProvider(ticket.ProgramId).Returns(provider);

        // Act
        var result = generateMessageService.CreateMessageContent(ticket, languageCode, invalidLevel);

        // Assert
        Assert.Equal(string.Empty, result);
        provider.Received(1).GetTemplate(invalidLevel, languageCode, ticket);
    }

    [Fact]
    public void CreateMessageItems_ShouldReturnMemberMessageItems_WhenLevelIsMember()
    {
        // Arrange
        var tickets = GetSampleTickets();
        const int messageId = 1;
        const string level = Levels.Member;

        // Act
        var result = generateMessageService.CreateMessageItems(tickets, messageId, level);

        // Assert
        Assert.All(result, item => Assert.Contains(tickets, ticket => ticket.CustId == item.TargetId && ticket.Username == item.TargetName));
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void CreateMessageItems_ShouldReturnAgentAndAboveMessageItems_WhenLevelIsNotMember()
    {
        // Arrange
        var tickets = GetSampleTickets();
        const int messageId = 1;
        const string level = Levels.Agent;

        // Act
        var result = generateMessageService.CreateMessageItems(tickets, messageId, level);

        // Assert
        Assert.All(result, item =>
            Assert.Contains(tickets, ticket =>
                (ticket.AgentId == item.TargetId && ticket.AgentUsername == item.TargetName) ||
                (ticket.MasterId == item.TargetId && ticket.MasterUsername == item.TargetName) ||
                (ticket.SuperId == item.TargetId && ticket.SuperUsername == item.TargetName)
            )
        );
    }

    private static List<SabaWalletTicket> GetSampleTickets()
    {
        return
        [
            new SabaWalletTicket
            {
                CustId = 1,
                Username = "User1",
                TransId = 1,
                AgentId = 10,
                AgentUsername = "Agent1",
                MasterId = 100,
                MasterUsername = "Master1",
                SuperId = 1000,
                SuperUsername = "Super1"
            },

            new SabaWalletTicket
            {
                CustId = 1,
                Username = "User1",
                TransId = 2,
                AgentId = 10,
                AgentUsername = "Agent1",
                MasterId = 100,
                MasterUsername = "Master1",
                SuperId = 1000,
                SuperUsername = "Super1"
            },

            new SabaWalletTicket
            {
                CustId = 2,
                Username = "User2",
                TransId = 3,
                AgentId = 20,
                AgentUsername = "Agent2",
                MasterId = 200,
                MasterUsername = "Master2",
                SuperId = 2000,
                SuperUsername = "Super2"
            }
        ];
    }

    public static IEnumerable<object[]> CreateMessageContentInputData()
    {
        return [
            [Levels.Member, ContentLanguage.English, MessageContent.MemberEng],
            [Levels.Member, ContentLanguage.Mandarin, MessageContent.MemberMandarin],
            [Levels.Agent, ContentLanguage.English, MessageContent.SMAEng],
            [Levels.Agent, ContentLanguage.Cantonese, MessageContent.SMACantonese]
        ];
    }
}