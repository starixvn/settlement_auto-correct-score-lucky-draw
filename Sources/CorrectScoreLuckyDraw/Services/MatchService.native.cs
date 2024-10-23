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
		return (await repository.FetchAsync<Match>(criteria)).ToList();
	}
}