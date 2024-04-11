using CorrectScoreLuckyDraw.Constants;
using CorrectScoreLuckyDraw.Models;

namespace CorrectScoreLuckyDraw.Services;

public interface AutoNotificationController
{
    Task Process();
}

public class AutoNotificationControllerImp(
    NotificationService notificationService,
    SiteMessageService siteMessageService) : AutoNotificationController
{
    public async Task Process()
    {
        var notifications = await notificationService.GetNotification();
        var drawIds = await InsertMessage(notifications);
        notificationService.SendMail(notifications);
        await UpdateNotification(drawIds);
    }

    private async Task<List<int>> InsertMessage(List<Ticket> tickets)
    {
        var drawIds = new List<int>();
        foreach (var item in tickets)
        {
            var result = await MessageBySite(item);

            if (result.ErrorCode == 0)
            {
                drawIds.Add(item.DrawId);
            }
        }

        return drawIds;
    }

    private async Task<Result> MessageBySite(Ticket item)
    {
        return item.Site.Equals(Site.IBCIndo)
            ? await siteMessageService.SiteIBCIndo(item)
            : await siteMessageService.SiteNotIBCIndo(item);
    }

    private async Task UpdateNotification(IReadOnlyCollection<int> drawIds)
    {
        if (drawIds.Count > 0)
        {
            await notificationService.UpdateNotification(string.Join(",", drawIds));
        }
    }
}