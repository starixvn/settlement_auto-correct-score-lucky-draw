using CorrectScoreLuckyDraw.Constants;
using CorrectScoreLuckyDraw.Services;

namespace CorrectScoreLuckyDraw.Models;

public class Ticket
{
    public RefDataService RefDataService { get; set; }

    public int DrawId { get; set; }

    public int MatchId { get; set; }

    public int TypeId { get; set; }

    public DateTime KickOffTime { get; set; }

    public int MatchCode { get; set; }

    public int HomeId { get; set; }

    public int AwayId { get; set; }

    public int CustId { get; set; }

    public string Username { get; set; }

    public int? AgentId { get; set; }

    public string AgentUsername { get; set; }

    public int? MasterId { get; set; }

    public string MasterUsername { get; set; }

    public int? SuperId { get; set; }

    public string SuperUsername { get; set; }

    public string Site { get; set; }

    public string CurrencyName { get; set; }

    public decimal PrizeAmount { get; set; }

    public string VerificationCode { get; set; }

    public DateTime ExpiredDate { get; set; }

    public DateTime LastModifiedDate { get; set; }

    public bool IsTest { get; set; }

    public string TeamName => $"{RefDataService.GetTeamName(HomeId, ContentLanguage.English)} -vs- {RefDataService.GetTeamName(AwayId, ContentLanguage.English)}";

    public string TeamNameCh => GetTeamName(ContentLanguage.Cantonese);

    public string TeamNameCs => GetTeamName(ContentLanguage.Mandarin);

    private string GetTeamName(string languageCode)
    {
        var home = RefDataService.GetTeamName(HomeId, languageCode);
        var away = RefDataService.GetTeamName(AwayId, languageCode);

        return (string.IsNullOrEmpty(home) || string.IsNullOrEmpty(away)) ? TeamName : $"{home} -vs- {away}";
    }
}