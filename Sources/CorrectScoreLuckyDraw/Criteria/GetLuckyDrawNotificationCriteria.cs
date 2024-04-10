namespace CorrectScoreLuckyDraw.Criteria;

public class GetLuckyDrawNotificationCriteria : CriteriaBase
{
    public override string GetSettingKey() => "NAP_LuckyDrawNotification_Select";

    public override bool IsValid() => true;
}