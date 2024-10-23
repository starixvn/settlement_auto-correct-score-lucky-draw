using Fanex.RemotingRef;

namespace Core.Services;

public interface RefDataService
{
    string GetLeagueName(int leagueId, string languageCode);

    string GetTeamName(int teamId, string languageCode);

    string GetProgramName(int programId, string languageCode);
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

    public string GetProgramName(int programId, string languageCode)
    {
        return remotingRefService.GetResourceDataByRefIdAndResourceId("RewardProgram", programId.ToString(), languageCode, "ProgramName");
    }
}