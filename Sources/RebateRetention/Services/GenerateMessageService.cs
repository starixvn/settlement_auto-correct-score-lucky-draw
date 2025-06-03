using Core.Constants;
using Core.Models;
using RebateRetention.Models;

namespace RebateRetention.Services;

public interface GenerateMessageService
{
    IEnumerable<MessageItem> CreateMessageItems(IEnumerable<SabaWalletTicket> tickets, int messageId, string level);

    string CreateMessageContent(SabaWalletTicket ticket, string languageCode, string level);
}

public class GenerateMessageServiceImp(
    MessageTemplateProviderFactory templateProviderFactory)
    : GenerateMessageService
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
        var provider = templateProviderFactory.GetProvider(ticket.ProgramId);
        return provider.GetTemplate(level, languageCode, ticket);
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
}