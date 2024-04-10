namespace CorrectScoreLuckyDraw.Criteria;

public class GetLuckyDrawMatchCriteria : CriteriaBase
{
    public override string GetSettingKey() => "NAP_Settlement_LuckyDrawMatch_Select";

    public override bool IsValid() => true;
}