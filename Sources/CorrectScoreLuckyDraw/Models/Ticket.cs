namespace CorrectScoreLuckyDraw.Models;

public class Ticket
{
    public int DrawId { get; set; }

    public int MatchId { get; set; }

    public int TypeId { get; set; }

    public DateTime KickOffTime { get; set; }

    public int HomeId { get; set; }

    public int AwayId { get; set; }

    public int CustId { get; set; }

    public string Username { get; set; }

    public int AgentId { get; set; }

    public string AgentUsername { get; set; }

    public int MasterId { get; set; }

    public string MasterUsername { get; set; }

    public int SuperId { get; set; }

    public string SuperUsername { get; set; }

    public string Site { get; set; }

    public string CurrencyName { get; set; }

    public decimal PrizeAmount { get; set; }

    public string VerificationCode { get; set; }

    public DateTime ExpiredDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool IsTest { get; set; }
}