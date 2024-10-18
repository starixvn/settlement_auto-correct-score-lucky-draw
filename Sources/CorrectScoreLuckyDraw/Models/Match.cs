using Core.Constants;
using Core.Services;

namespace CorrectScoreLuckyDraw.Models;

public class Match
{
    public RefDataService RefDataService { get; set; }

    public int MatchId { get; set; }

    public decimal MinBet { get; set; }

    public bool IsTest { get; set; }

    public decimal MinPool { get; set; }

    public decimal MaxPool { get; set; }

    public int TotalWinner { get; set; }

    public decimal TotalPool { get; set; }

    public DateTime CreatedDate { get; set; }

    public DateTime KickOffTime { get; set; }

    public int HomeId { get; set; }

    public int AwayId { get; set; }

    public string TeamName => $"{RefDataService.GetTeamName(HomeId, ContentLanguage.English)} vs {RefDataService.GetTeamName(AwayId, ContentLanguage.English)}";
}