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
            if (IsExistMember(item))
            {
                await HandleInsSabaMsgForLevel(item, Levels.Member);
            }

            if (IsExistSMA(item))
            {
                await HandleInsSabaMsgForLevel(item, Levels.Agent);
            }

            await sabaProgramStatusService.UpdateSabaProgramStatus(item.First().SId);
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
            MsgContentsType = 100,
            MsgType = 1,
            Creator = Creator.Id,
            CreatorName = "starixIT",
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