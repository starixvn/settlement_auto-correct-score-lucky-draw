using Core.Constants;
using Core.Criteria;
using Core.Services;
using RebateRetention.Models;

namespace RebateRetention.Services;

public interface AutoRebateRetentionController
{
	Task Process();
}

public class AutoRebateRetentionControllerImp : AutoRebateRetentionController
{
	private readonly SabaWalletService sabaWalletService;
	private readonly IMessageService messageService;
	private readonly GenerateMessageService generateMessageService;

	public AutoRebateRetentionControllerImp(
		SabaWalletService sabaWalletService,
		IMessageService messageService,
		GenerateMessageService generateMessageService)
	{
		this.sabaWalletService = sabaWalletService;
		this.messageService = messageService;
		this.generateMessageService = generateMessageService;
	}

	public async Task Process()
	{
		var tickets = (await sabaWalletService.GetSabaWalletTickets()).ToList();

		foreach (var item in tickets.GroupBy(x => x.ProgramId))
		{
			if (IsExistMember(item))
			{
				var messageId = await CreateMsg(item.First(), Levels.Member);
				var messageItems = generateMessageService.CreateMemberMessageItems(item, messageId);
				await messageService.UploadBatchMessage(messageItems);
			}

			if (IsExistSMA(item))
			{
				var messageId = await CreateMsg(item.First(), Levels.Agent);
				var messageItems = generateMessageService.CreateSMAMessageItems(item, messageId);
				await messageService.UploadBatchMessage(messageItems);
			}
		}
	}

	private async Task<int> CreateMsg(SabaWalletTicket ticket, string level)
	{
		var msgInsCriteria = new MsgInsCriteria
		{
			MsgContentsType = 200,
			MsgType = 1,
			ContentEng = generateMessageService.CreateMessageContent(ticket, ContentLanguage.English, level),
			ContentCh = generateMessageService.CreateMessageContent(ticket, ContentLanguage.Cantonese, level),
			ContentCs = generateMessageService.CreateMessageContent(ticket, ContentLanguage.Mandarin, level),
			ContentId = generateMessageService.CreateMessageContent(ticket, ContentLanguage.Indo, level),
		};

		var messageId = await messageService.CreateMessage(msgInsCriteria);

		return messageId;
	}

	private bool IsExistMember(IEnumerable<SabaWalletTicket> tickets)
		=> tickets.Any(x => x.CustId != 0);

	private bool IsExistSMA(IEnumerable<SabaWalletTicket> tickets)
		=> tickets.Any(x => x.AgentId != 0 || x.MasterId != 0 || x.SuperId != 0);
}