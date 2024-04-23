using Fanex.RemotingRef;

namespace CorrectScoreLuckyDraw.Services;

public interface RefDataService
{
    string GetLeagueName(int leagueId, string languageCode);

    string GetTeamName(int teamId, string languageCode);
}

public class RefDataServiceImp(IRemotingRefService remotingRefService) : RefDataService
{
    public string GetLeagueName(int leagueId, string languageCode)
    {
        return remotingRefService.GetLeagueName(leagueId.ToString(), languageCode, string.Empty);
    }

    public string GetTeamName(int teamId, string languageCode)
    {
        return remotingRefService.GetTeamName(teamId.ToString(), languageCode, string.Empty);
    }
}