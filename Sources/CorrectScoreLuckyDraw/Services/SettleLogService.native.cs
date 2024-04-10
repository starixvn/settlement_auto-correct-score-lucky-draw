namespace CorrectScoreLuckyDraw.Services;

using Criteria;

public interface SettleLogService
{
    Task Log(int matchId, string remark);
}

public class SettleLogServiceImp(IDynamicRepository repository) : SettleLogService
{
    public async Task Log(int matchId, string remark)
    {
        var criteria = new WriteLogCriteria
        {
            MatchId = matchId,
            Remark = remark
        };
        await repository.ExecuteAsync(criteria);
    }
}