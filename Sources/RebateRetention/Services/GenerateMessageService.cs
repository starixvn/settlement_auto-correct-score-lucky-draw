using Core.Constants;
using Core.Models;
using Core.Services;
using RebateRetention.Constants;
using RebateRetention.Models;

namespace RebateRetention.Services;

public interface GenerateMessageService
{
	IEnumerable<MessageItem> CreateMemberMessageItems(IEnumerable<SabaWalletTicket> tickets, int messageId);

	IEnumerable<MessageItem> CreateSMAMessageItems(IEnumerable<SabaWalletTicket> tickets, int messageId);

	string CreateMessageContent(SabaWalletTicket ticket, string languageCode, string level);
}

public class GenerateMessageServiceImp : GenerateMessageService
{
	private readonly RefDataService refDataService;

	public GenerateMessageServiceImp(RefDataService refDataService)
	{
		this.refDataService = refDataService;
	}

	public IEnumerable<MessageItem> CreateSMAMessageItems(IEnumerable<SabaWalletTicket> tickets, int messageId)
	{
		var groupAgent = GenerateMessageItems(tickets, ticket => ticket.AgentId, ticket => ticket.AgentUsername, messageId);
		var groupMaster = GenerateMessageItems(tickets, ticket => ticket.MasterId, ticket => ticket.MasterUsername, messageId);
		var groupSuper = GenerateMessageItems(tickets, ticket => ticket.SuperId, ticket => ticket.SuperUsername, messageId);

		return groupAgent
			.Concat(groupMaster)
			.Concat(groupSuper);
	}

	public IEnumerable<MessageItem> CreateMemberMessageItems(IEnumerable<SabaWalletTicket> tickets, int messageId)
	{
		var groupMembers = GenerateMessageItems(tickets, ticket => ticket.CustId, ticket => ticket.Username, messageId);

		return groupMembers;
	}

	public string CreateMessageContent(SabaWalletTicket ticket, string languageCode, string level)
	{
		var template = GetTemplate(ticket.ProgramId, level, languageCode);
		var programName = refDataService.GetProgramName(ticket.ProgramId, languageCode);

		return template
			.Replace("@program_name", programName);
	}

	private static IEnumerable<MessageItem> GenerateMessageItems(
		IEnumerable<SabaWalletTicket> tickets,
		Func<SabaWalletTicket, int> idSelector,
		Func<SabaWalletTicket, string> nameSelector,
		int messageId)
	{
		return tickets
			.Where(ticket => idSelector(ticket) != 0)
			.GroupBy(idSelector)
			.Select(group => new MessageItem
			{
				TargetId = group.Key,
				TargetName = nameSelector(group.First()),
				BetTransId = string.Join(",", group.Select(ticket => ticket.TransId).Distinct()),
				MessageId = messageId
			})
			.ToList();
	}

	private static string GetTemplate(int programId, string level, string languageCode)
	{
		return (programId, level) switch
		{
			(1, Levels.Member) => GetTemplateForMember(1, languageCode),
			(1, Levels.Agent) => GetTemplateForSMA(1, languageCode),
			_ => string.Empty
		};
	}

	private static string GetTemplateForMember(int programId, string languageCode)
	{
		return (programId, languageCode) switch
		{
			(_, ContentLanguage.English) => MsgContent.MemberEng,
			(_, ContentLanguage.Cantonese) => MsgContent.MemberCantonese,
			(_, ContentLanguage.Mandarin) => MsgContent.MemberMandarin,
			(_, ContentLanguage.Indo) => MsgContent.MemberIndo,
			_ => string.Empty
		};
	}

	private static string GetTemplateForSMA(int programId, string languageCode)
	{
		return (programId, languageCode) switch
		{
			(_, ContentLanguage.English) => MsgContent.SMAEng,
			(_, ContentLanguage.Cantonese) => MsgContent.SMACantonese,
			(_, ContentLanguage.Mandarin) => MsgContent.SMAMandarin,
			(_, ContentLanguage.Indo) => MsgContent.SMAIndo,
			_ => string.Empty
		};
	}
}