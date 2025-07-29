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

        foreach (var programTickets in tickets.GroupBy(x => x.ProgramId))
        {
            var firstTicket = programTickets.FirstOrDefault();
            if (firstTicket is null)
            {
                continue;
            }

            if (firstTicket.TransId == 0)
            {
                await sabaProgramStatusService.UpdateSabaProgramStatus(firstTicket.SId);
                return;
            }

            await ProcessTicketsForLevelsAsync(programTickets);
            await sabaProgramStatusService.UpdateSabaProgramStatus(firstTicket.SId);
        }
    }

    private async Task ProcessTicketsForLevelsAsync(IGrouping<int, SabaWalletTicket> ticketsByProgram)
    {
        if (IsExistMember(ticketsByProgram))
        {
            await HandleInsertSabaMsgForLevelAsync(ticketsByProgram, Levels.Member);
        }

        if (IsExistSMA(ticketsByProgram))
        {
            await HandleInsertSabaMsgForLevelAsync(ticketsByProgram, Levels.Agent);
        }
    }

    private async Task HandleInsertSabaMsgForLevelAsync(IGrouping<int, SabaWalletTicket> ticketsByProgram, string level)
    {
        var messageId = await CreateMessageAsync(ticketsByProgram.First(), level);
        var messageItems = generateMessageService.CreateMessageItems(ticketsByProgram, messageId, level).ToList();
        await messageService.UploadBatchMessage(messageItems);
    }

    private async Task<int> CreateMessageAsync(SabaWalletTicket ticket, string level)
    {
        var msgInsCriteria = new InsertMessageCriteria
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

    private static bool IsExistMember(IEnumerable<SabaWalletTicket> tickets)
        => tickets.Any(x => x.CustId != 0);

    private static bool IsExistSMA(IEnumerable<SabaWalletTicket> tickets)
        => tickets.Any(x => x.AgentId != 0 || x.MasterId != 0 || x.SuperId != 0);
}