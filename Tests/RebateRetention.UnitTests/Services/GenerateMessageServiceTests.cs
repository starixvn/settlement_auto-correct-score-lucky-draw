using Core.Constants;
using Core.Services;
using Fanex.Data.Repository;
using Fanex.Settlement.Converter.Services;
using NSubstitute;
using RebateRetention.Constants;
using RebateRetention.Models;
using RebateRetention.Services;

namespace RebateRetention.UnitTests.Services;

public class GenerateMessageServiceTests
{
	private readonly RefDataService refDataService;
	private readonly GenerateMessageService generateMessageService;

	public GenerateMessageServiceTests()
	{
		refDataService = Substitute.For<RefDataService>();
		generateMessageService = new GenerateMessageServiceImp(refDataService);
	}

	[Fact]
	public void CreateMessageContent_ShouldReturnMessageWithReplacedProgramName_WhenValidInputsProvided()
	{
		// Arrange
		var ticket = new SabaWalletTicket
		{
			ProgramId = 1,
		};
		string languageCode = ContentLanguage.English;
		string level = Levels.Member;
		string expectedProgramName = "Test Program";

		refDataService.GetProgramName(ticket.ProgramId, languageCode).Returns(expectedProgramName);

		var expectedTemplate = MsgContent.MemberEng.Replace("@program_name", expectedProgramName);

		// Act
		var result = generateMessageService.CreateMessageContent(ticket, languageCode, level);

		// Assert
		Assert.Equal(expectedTemplate, result);
		refDataService.Received(1).GetProgramName(ticket.ProgramId, languageCode);
	}

	[Theory]
	[InlineData(Levels.Member, ContentLanguage.English, MsgContent.MemberEng)]
	[InlineData(Levels.Member, ContentLanguage.Mandarin, MsgContent.MemberMandarin)]
	[InlineData(Levels.Agent, ContentLanguage.English, MsgContent.SMAEng)]
	[InlineData(Levels.Agent, ContentLanguage.Cantonese, MsgContent.SMACantonese)]
	public void CreateMessageContent_ShouldReturnCorrectTemplateBasedOnInputs(string level, string languageCode, string expectedTemplate)
	{
		// Arrange
		var ticket = new SabaWalletTicket
		{
			ProgramId = 1,
		};
		string programName = "Test Program";

		refDataService.GetProgramName(ticket.ProgramId, languageCode).Returns(programName);

		var expectedMessage = expectedTemplate.Replace("@program_name", programName);

		// Act
		var result = generateMessageService.CreateMessageContent(ticket, languageCode, level);

		// Assert
		Assert.Equal(expectedMessage, result);
		refDataService.Received(1).GetProgramName(ticket.ProgramId, languageCode);
	}

	[Fact]
	public void CreateMessageContent_ShouldReturnEmptyString_WhenInvalidLevelProvided()
	{
		// Arrange
		var ticket = new SabaWalletTicket
		{
			ProgramId = 1,
		};
		string languageCode = ContentLanguage.English;
		string invalidLevel = "InvalidLevel";

		// Act
		var result = generateMessageService.CreateMessageContent(ticket, languageCode, invalidLevel);

		// Assert
		Assert.Equal(string.Empty, result);
	}

	[Fact]
	public void CreateMessageItems_ShouldReturnMemberMessageItems_WhenLevelIsMember()
	{
		// Arrange
		var tickets = GetSampleTickets();
		int messageId = 1;
		string level = Levels.Member;

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
		int messageId = 1;
		string level = Levels.Agent;

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

	private List<SabaWalletTicket> GetSampleTickets()
	{
		return new List<SabaWalletTicket>
		{
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
		};
	}
}