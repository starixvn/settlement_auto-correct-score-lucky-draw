namespace CorrectScoreLuckyDraw.Criteria;

public class GetLuckyDrawMemberCriteria : CriteriaBase
{
    public int MatchId { get; set; }

    public int MinBet { get; set; }

    public bool IsTest { get; set; }

    public override string GetSettingKey() => "NAP_Settlement_LuckyDrawMember_Select";

    public override bool IsValid() => MatchId > 0;
}