using CorrectScoreLuckyDraw.Criteria;
using CorrectScoreLuckyDraw.Models;

namespace CorrectScoreLuckyDraw.Services;

public interface NotificationService
{
    Task<List<Ticket>> GetNotification();

    Task UpdateNotification(string drawIdList);

    Task SendNotification(List<Ticket> tickets);
}

public class NotificationServiceImp(IDynamicRepository repository) : NotificationService
{
    public async Task<List<Ticket>> GetNotification()
    {
        var criteria = new GetLuckyDrawNotificationCriteria();

        return
        [
            new Ticket
            {
                MatchId = 12345,
                DrawId = 11,
                CustId = 8,
                Username = "sa",
                AgentId = 9,
                AgentUsername = "Agent",
                MasterId = 10,
                MasterUsername = "Master",
                SuperId = 11,
                SuperUsername = "Super",
                Site = "IBCIndo 1",
                CurrencyName = "RM",
                PrizeAmount = 2,
                VerificationCode = "123123",
                ExpiredDate = DateTime.Now
            },

            new Ticket
            {
                MatchId = 1234567,
                DrawId = 22,
                CustId = 8,
                Username = "sa",
                AgentId = 9,
                AgentUsername = "Agent",
                MasterId = 10,
                MasterUsername = "Master",
                SuperId = 11,
                SuperUsername = "Super",
                Site = "IBCIndo",
                CurrencyName = "RM",
                PrizeAmount = 2,
                VerificationCode = "123123",
                ExpiredDate = DateTime.Now
            }
        ];
        // return (await repository.FetchAsync<Ticket>(criteria)).ToList();
    }

    public async Task UpdateNotification(string drawIdList)
    {
        var criteria = new UpdateLuckyDrawNotificationCriteria()
        {
            DrawIdList = drawIdList
        };
        //await repository.ExecuteAsync(criteria);
    }

    public async Task SendNotification(List<Ticket> tickets)
    {
    }
}