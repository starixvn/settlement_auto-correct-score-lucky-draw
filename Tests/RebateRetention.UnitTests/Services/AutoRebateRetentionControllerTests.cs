using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Constants;
using Core.Criteria;
using Core.Models;
using Core.Services;
using NSubstitute;
using RebateRetention.Models;
using RebateRetention.Services;

namespace RebateRetention.Tests.Services;

public class AutoRebateRetentionControllerTests
{
	private readonly SabaWalletService sabaWalletService;
	private readonly IMessageService messageService;
	private readonly GenerateMessageService generateMessageService;
	private readonly SabaProgramStatusService sabaProgramStatusService;
	private readonly AutoRebateRetentionControllerImp controller;

	public AutoRebateRetentionControllerTests()
	{
		sabaWalletService = Substitute.For<SabaWalletService>();
		messageService = Substitute.For<IMessageService>();
		generateMessageService = Substitute.For<GenerateMessageService>();
		sabaProgramStatusService = Substitute.For<SabaProgramStatusService>();

		controller = new AutoRebateRetentionControllerImp(
			sabaWalletService,
			messageService,
			generateMessageService,
			sabaProgramStatusService);
	}

	[Fact]
	public async Task Process_ShouldCallHandleInsSabaMsgForMember_AndUpdateSabaProgramStatus_WhenTicketsHaveMembers()
	{
		// Arrange
		var tickets = new List<SabaWalletTicket>
		{
			new SabaWalletTicket { ProgramId = 1, CustId = 1001, SId = 1, TransId = 1 }
		};

		sabaWalletService.GetSabaWalletTickets().Returns(tickets);

		generateMessageService.CreateMessageItems(Arg.Any<IEnumerable<SabaWalletTicket>>(), Arg.Any<int>(), Arg.Any<string>())
			.Returns(new List<MessageItem> { new MessageItem { MessageId = 1, TargetId = 1001 } });

		messageService.CreateMessage(Arg.Any<MsgInsCriteria>()).Returns(1);

		// Act
		await controller.Process();

		// Assert
		await sabaWalletService.Received(1).GetSabaWalletTickets();
		await messageService.Received(1).UploadBatchMessage(Arg.Any<List<MessageItem>>());
		await sabaProgramStatusService.Received(1).UpdateSabaProgramStatus(1);
	}

	[Fact]
	public async Task Process_ShouldCallHandleInsSabaMsgForSMA_AndUpdateSabaProgramStatus_WhenTicketsHaveSMA()
	{
		// Arrange
		var tickets = new List<SabaWalletTicket>
		{
			new SabaWalletTicket { ProgramId = 2, AgentId = 2001, SId = 2, TransId = 2 }
		};

		sabaWalletService.GetSabaWalletTickets().Returns(tickets);

		generateMessageService.CreateMessageItems(Arg.Any<IEnumerable<SabaWalletTicket>>(), Arg.Any<int>(), Arg.Any<string>())
			.Returns(new List<MessageItem> { new MessageItem { MessageId = 1, TargetId = 2001 } });

		messageService.CreateMessage(Arg.Any<MsgInsCriteria>()).Returns(1);

		// Act
		await controller.Process();

		// Assert
		await sabaWalletService.Received(1).GetSabaWalletTickets();
		await messageService.Received(1).UploadBatchMessage(Arg.Any<List<MessageItem>>());
		await sabaProgramStatusService.Received(1).UpdateSabaProgramStatus(2);
	}

	[Fact]
	public async Task Process_ShouldHandleBothMembersAndSMA_WhenTicketsHaveBoth()
	{
		// Arrange
		var tickets = new List<SabaWalletTicket>
		{
			new SabaWalletTicket { ProgramId = 3, CustId = 1001, AgentId = 2001, SId = 3, TransId =3 }
		};

		sabaWalletService.GetSabaWalletTickets().Returns(tickets);

		generateMessageService.CreateMessageItems(Arg.Any<IEnumerable<SabaWalletTicket>>(), Arg.Any<int>(), Arg.Any<string>())
			.Returns(new List<MessageItem> { new MessageItem { MessageId = 1, TargetId = 1001 }, new MessageItem { MessageId = 1, TargetId = 2001 } });

		messageService.CreateMessage(Arg.Any<MsgInsCriteria>()).Returns(1);

		// Act
		await controller.Process();

		// Assert
		await sabaWalletService.Received(1).GetSabaWalletTickets();
		await messageService.Received(2).UploadBatchMessage(Arg.Any<List<MessageItem>>());
		await sabaProgramStatusService.Received(1).UpdateSabaProgramStatus(3);
	}
}