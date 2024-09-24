using Core.Services;
using CorrectScoreLuckyDraw.Criteria;
using CorrectScoreLuckyDraw.Models;

namespace CorrectScoreLuckyDraw.Services;

public interface NotificationService
{
    Task<List<Ticket>> GetNotification();

    Task UpdateNotification(string drawIdList);

    void SendMail(List<Ticket> tickets);
}

public class NotificationServiceImp(
    IDynamicRepository repository,
    RefDataService refDataService,
    SendMailService sendMailService) : NotificationService
{
    public async Task<List<Ticket>> GetNotification()
    {
        var criteria = new GetLuckyDrawNotificationCriteria();
        return (await repository.FetchAsync<Ticket>(criteria)).ToList();
    }

    public async Task UpdateNotification(string drawIdList)
    {
        var criteria = new UpdateLuckyDrawNotificationCriteria
        {
            DrawIdList = drawIdList
        };
        await repository.ExecuteAsync(criteria);
    }

    public void SendMail(List<Ticket> tickets)
    {
        var listNotification = tickets.GroupBy(x => x.MatchId).Select(n => new NotificationModel
        {
            Match = new Match
            {
                MatchId = n.Key,
                HomeId = n.First().HomeId,
                AwayId = n.First().AwayId,
                KickOffTime = n.First().KickOffTime,
                CreatedDate = n.First().CreatedDate,
                RefDataService = refDataService,
                IsTest = n.First().IsTest
            },
            Tickets = n
        }).ToList();

        foreach (var item in listNotification)
        {
            sendMailService.SendMail(item);
        }
    }
}