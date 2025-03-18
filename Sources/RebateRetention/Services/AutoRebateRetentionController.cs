using Core.Constants;
using Core.Criteria;
using Core.Services;
using RebateRetention.Models;

namespace RebateRetention.Services;

public interface AutoRebateRetentionController
{
    Task Process();
}

public class AutoRebateRetentionControllerImp(
    SabaWalletService sabaWalletService,
    IMessageService messageService,
    GenerateMessageService generateMessageService,
    SabaProgramStatusService sabaProgramStatusService)
    : AutoRebateRetentionController
{
    public async Task Process()
    {
        var tickets = await sabaWalletService.GetSabaWalletTickets();

        foreach (var item in tickets.GroupBy(x => x.ProgramId))
        {
            var firstItem = item.First();

            if (firstItem is { TransId: 0 })
            {
                await sabaProgramStatusService.UpdateSabaProgramStatus(item.First().SId);
                return;
            }

            await ProcessTicketsForLevels(item);

            await sabaProgramStatusService.UpdateSabaProgramStatus(firstItem!.SId);
        }
    }

    private async Task ProcessTicketsForLevels(IGrouping<int, SabaWalletTicket> item)
    {
        if (IsExistMember(item))
        {
            await HandleInsSabaMsgForLevel(item, Levels.Member);
        }

        if (IsExistSMA(item))
        {
            await HandleInsSabaMsgForLevel(item, Levels.Agent);
        }
    }

    private async Task HandleInsSabaMsgForLevel(IGrouping<int, SabaWalletTicket> item, string level)
    {
        var messageId = await CreateMsg(item.First(), level);
        var messageItems = generateMessageService.CreateMessageItems(item, messageId, level).ToList();
        await messageService.UploadBatchMessage(messageItems);
    }

    private async Task<int> CreateMsg(SabaWalletTicket ticket, string level)
    {
        var msgInsCriteria = new MsgInsCriteria
        {
            MsgContentsType = 0,
            MsgType = 1,
            Creator = Creator.Id,
            CreatorName = Creator.Name,
            MatchId = null,
            ContentEng = generateMessageService.CreateMessageContent(ticket, ContentLanguage.English, level),
            ContentCh = generateMessageService.CreateMessageContent(ticket, ContentLanguage.Cantonese, level),
            ContentCs = generateMessageService.CreateMessageContent(ticket, ContentLanguage.Mandarin, level),
            ContentThai = generateMessageService.CreateMessageContent(ticket, ContentLanguage.Thai, level)
        };

        var messageId = await messageService.CreateMessage(msgInsCriteria);

        return messageId;
    }

    private bool IsExistMember(IEnumerable<SabaWalletTicket> tickets)
        => tickets.Any(x => x.CustId != 0);

    private bool IsExistSMA(IEnumerable<SabaWalletTicket> tickets)
        => tickets.Any(x => x.AgentId != 0 || x.MasterId != 0 || x.SuperId != 0);
}