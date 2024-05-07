using CorrectScoreLuckyDraw.Criteria;
using CorrectScoreLuckyDraw.Models;

namespace CorrectScoreLuckyDraw.Services;

public interface MemberService
{
    Task<List<Member>> GetMembersByMatchId(Match match);

    Task InsertMembers(Match match, string members);
}

public class MemberServiceImp(IDynamicRepository repository) : MemberService
{
    public async Task<List<Member>> GetMembersByMatchId(Match match)
    {
        var criteria = new GetLuckyDrawMemberCriteria
        {
            MatchId = match.MatchId,
            MinBet = match.MinBet,
            IsTest = match.IsTest
        };
        return (await repository.FetchAsync<Member>(criteria)).ToList();
    }

    public async Task InsertMembers(Match match, string members)
    {
        var poolAmount = GetPoolAmount(match);
        var criteria = new InsertLuckyDrawMemberCriteria
        {
            MatchId = match.MatchId,
            ListTransIds = members,
            IsTest = match.IsTest,
            TotalWinner = match.TotalWinner,
            PoolAmount = poolAmount
        };

        await repository.ExecuteAsync(criteria);
    }

    private static decimal GetPoolAmount(Match match)
    {
        var minPool = match.TotalPool + match.MinPool;

        return minPool <= match.MaxPool ? minPool : match.MaxPool;
    }
}