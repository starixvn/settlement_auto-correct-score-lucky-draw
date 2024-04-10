namespace CorrectScoreLuckyDraw.Models;

public class Match
{
    public int MatchId { get; set; }

    public int LeagueId { get; set; }

    public int MinBet { get; set; }

    public bool IsTest { get; set; }

    public decimal MinPool { get; set; }

    public decimal MaxPool { get; set; }

    public int TotalWinner { get; set; }

    public decimal TotalPool { get; set; }
}