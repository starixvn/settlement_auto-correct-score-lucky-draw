using Core.Constants;
using Core.Models;
using Core.Services;
using CorrectScoreLuckyDraw.Models;

namespace CorrectScoreLuckyDraw.Services;

public interface SiteMessageService
{
    Task<Result> SiteNotIBCIndo(Ticket ticket);

    Task<Result> SiteIBCIndo(Ticket ticket);
}

public class SiteMessageServiceImp(MessageAnnouncementService messageAnnouncementService, IMessageService messageService) : SiteMessageService
{
    public async Task<Result> SiteNotIBCIndo(Ticket ticket)
    {
        var messageId = await GetMessageId(ticket, string.Empty);
        var item = messageAnnouncementService.CreateMessageItem(ticket, messageId, string.Empty);
        return await messageService.UploadBatchMessage([item]);
    }

    public async Task<Result> SiteIBCIndo(Ticket ticket)
    {
        var levels = new List<string> { Levels.Member, Levels.Agent, Levels.Master, Levels.Super };
        var listMessages = new List<MessageItem>();
        foreach (var level in levels)
        {
            var messageId = await GetMessageId(ticket, level);
            var message = messageAnnouncementService.CreateMessageItem(ticket, messageId, level);
            listMessages.Add(message);
        }

        return await messageService.UploadBatchMessage(listMessages);
    }

    private async Task<int> GetMessageId(Ticket ticket, string level)
    {
        var criteria = messageAnnouncementService.CreateMessageCriteria(ticket, level);

        return await messageService.CreateMessage(criteria);
    }
}