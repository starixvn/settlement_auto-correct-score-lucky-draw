using CorrectScoreLuckyDraw.Criteria;
using CorrectScoreLuckyDraw.Models;

namespace CorrectScoreLuckyDraw.Services;

public interface MatchService
{
    Task<List<Match>> GetMatch();
}

public class MatchServiceImp(IDynamicRepository repository) : MatchService
{
    public async Task<List<Match>> GetMatch()
    {
        var criteria = new GetLuckyDrawMatchCriteria();
        return
        [
            new Match
            {
                MatchId = 12345,
                LeagueId = 12345,
                TotalPool = 1,
                TotalWinner = 1,
                IsTest = true
            }
        ];

        //return (await repository.FetchAsync<Match>(criteria)).ToList();
    }
}