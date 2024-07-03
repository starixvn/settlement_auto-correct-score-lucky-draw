using CorrectScoreLuckyDraw.Constants;
using CorrectScoreLuckyDraw.Models;

namespace CorrectScoreLuckyDraw.Services;

public interface SiteMessageService
{
    Task<Result> SiteNotIBCIndo(Ticket ticket);

    Task<Result> SiteIBCIndo(Ticket ticket);
}

public class SiteMessageServiceImp(
    BatchMessageService messageService,
    MessageAnnouncementService messageAnnouncementService) : SiteMessageService
{
    public async Task<Result> SiteNotIBCIndo(Ticket ticket)
    {
        var messageId = await messageService.GetMessageId(ticket, string.Empty);
        var item = messageAnnouncementService.CreateMessageItem(ticket, messageId, string.Empty);
        return await messageService.UploadBatchMessage([item]);
    }

    public async Task<Result> SiteIBCIndo(Ticket ticket)
    {
        var levels = new List<string> { Levels.Member, Levels.Agent, Levels.Master, Levels.Super };
        var listMessages = new List<MessageItem>();
        foreach (var level in levels)
        {
            var messageId = await messageService.GetMessageId(ticket, level);
            var message = messageAnnouncementService.CreateMessageItem(ticket, messageId, level);
            listMessages.Add(message);
        }

        return await messageService.UploadBatchMessage(listMessages);
    }
}