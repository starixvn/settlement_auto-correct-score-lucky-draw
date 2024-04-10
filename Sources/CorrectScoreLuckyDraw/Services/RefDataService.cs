using Fanex.RemotingRef;

namespace CorrectScoreLuckyDraw.Services;

public interface RefDataService
{
    string GetTeamName(int teamId, string languageCode);
}

public class RefDataServiceImp(IRemotingRefService remotingRefService) : RefDataService
{
    public string GetTeamName(int teamId, string languageCode)
    {
        return remotingRefService.GetTeamName(teamId.ToString(), languageCode, string.Empty);
    }
}