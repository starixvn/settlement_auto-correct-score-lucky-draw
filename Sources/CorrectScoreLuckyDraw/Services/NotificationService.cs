using CorrectScoreLuckyDraw.Criteria;
using CorrectScoreLuckyDraw.Models;

namespace CorrectScoreLuckyDraw.Services;

public interface NotificationService
{
    Task<List<Ticket>> GetNotification();

    Task UpdateNotification(string drawIdList);
}

public class NotificationServiceImp(IDynamicRepository repository) : NotificationService
{
    public async Task<List<Ticket>> GetNotification()
    {
        var criteria = new GetLuckyDrawNotificationCriteria();

        return (await repository.FetchAsync<Ticket>(criteria)).ToList();
    }

    public async Task UpdateNotification(string drawIdList)
    {
        var criteria = new UpdateLuckyDrawNotificationCriteria()
        {
            DrawIdList = drawIdList
        };
        await repository.ExecuteAsync(criteria);
    }
}