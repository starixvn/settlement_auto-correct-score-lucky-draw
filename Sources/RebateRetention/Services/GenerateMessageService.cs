using Core.Constants;
using Core.Models;
using Core.Services;
using RebateRetention.Constants;
using RebateRetention.Models;

namespace RebateRetention.Services;

public interface GenerateMessageService
{
    IEnumerable<MessageItem> CreateMessageItems(IEnumerable<SabaWalletTicket> tickets, int messageId, string level);

    string CreateMessageContent(SabaWalletTicket ticket, string languageCode, string level);
}

public class GenerateMessageServiceImp(RefDataService refDataService) : GenerateMessageService
{
    public IEnumerable<MessageItem> CreateMessageItems(IEnumerable<SabaWalletTicket> tickets, int messageId, string level)
    {
        var groupMembers = GenerateMessageItems(tickets, ticket => ticket.CustId, ticket => ticket.Username, messageId);

        if (level == Levels.Member)
        {
            return groupMembers;
        }

        var groupAgent = GenerateMessageItems(tickets, ticket => ticket.AgentId, ticket => ticket.AgentUsername, messageId);
        var groupMaster = GenerateMessageItems(tickets, ticket => ticket.MasterId, ticket => ticket.MasterUsername, messageId);
        var groupSuper = GenerateMessageItems(tickets, ticket => ticket.SuperId, ticket => ticket.SuperUsername, messageId);

        return groupAgent
            .Concat(groupMaster)
            .Concat(groupSuper);
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
            (_, Levels.Member) => GetTemplateForMember(1, languageCode),
            (_, Levels.Agent) => GetTemplateForSMA(1, languageCode),
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