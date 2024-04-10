namespace CorrectScoreLuckyDraw.Criteria;

public class InsertLuckyDrawMemberCriteria : NonQueryCommand
{
    public int MatchId { get; set; }

    public string ListTransIds { get; set; }

    public bool IsTest { get; set; }

    public int TotalWinner { get; set; }

    public decimal PoolAmount { get; set; }

    public override string GetSettingKey() => "NAP_Settlement_LuckyDrawMember_Insert";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(ListTransIds);
}