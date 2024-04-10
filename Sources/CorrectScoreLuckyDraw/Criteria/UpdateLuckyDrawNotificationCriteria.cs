namespace CorrectScoreLuckyDraw.Criteria;

public class UpdateLuckyDrawNotificationCriteria : NonQueryCommand
{
    public string DrawIdList { get; set; }

    public override string GetSettingKey() => "NAP_LuckyDrawNotification_Upd";

    public override bool IsValid() => !string.IsNullOrWhiteSpace(DrawIdList);
}