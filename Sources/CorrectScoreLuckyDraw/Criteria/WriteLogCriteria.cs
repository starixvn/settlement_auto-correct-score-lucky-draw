using Action = Core.Constants.Action;

namespace CorrectScoreLuckyDraw.Criteria;

public class WriteLogCriteria : NonQueryCommand
{
    public int UserId { get; set; }

    public int MatchId { get; set; }

    public string Remark { get; set; }

    public string Ip { get; } = string.Empty;

    public int ActionId { get; set; } = Action.ActionId;

    public override bool IsValid() => MatchId > 0;

    public override string GetSettingKey() => "WriteLog_UserLog_Settle";
}