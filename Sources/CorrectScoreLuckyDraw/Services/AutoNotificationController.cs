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

        await UpdateNotification(drawIds);
    }

    private async Task SendMail(List<Ticket> tickets)
    {
    }

    private async Task<List<int>> InsertMessage(List<Ticket> tickets)
    {
        var drawIds = new List<int>();
        foreach (var item in tickets)
        {
            Result result;
            if (item.Site.Equals(Site.Name))
            {
                result = await siteMessageService.SiteIBCIndo(item);
            }
            else
            {
                result = await siteMessageService.SiteNotIBCIndo(item);
            }

            if (result.ErrorCode == 0)
            {
                drawIds.Add(item.DrawId);
            }
        }

        return drawIds;
    }

    private async Task UpdateNotification(IReadOnlyCollection<int> drawIds)
    {
        if (drawIds.Count > 0)
        {
            await notificationService.UpdateNotification(string.Join(",", drawIds));
        }
    }
}