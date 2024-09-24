using Fanex.Data;
using CreatorConst = Core.Constants.Creator;

namespace CorrectScoreLuckyDraw.Criteria;

public class InsertMessageCriteria : NonQueryCommand
{
	public int? MessageId { get; set; }

	[SpParam(SqlParamName = "Content_en")]
	public string ContentEng { get; set; }

	[SpParam(SqlParamName = "Content_ch")]
	public string ContentCh { get; set; }

	[SpParam(SqlParamName = "Content_cs")]
	public string ContentCs { get; set; }

	[SpParam(SqlParamName = "Content_id")]
	public string ContentId { get; set; }

	public int MatchId { get; set; }

	public short MsgType { get; set; } = 1;

	public int MsgContentsType { get; set; } = 112;

	public int Creator { get; set; } = CreatorConst.Id;

	public string CreatorName { get; set; } = string.Empty;

	public override string GetSettingKey() => "NAP_SpecialDelete_Message_Insert";

	public override bool IsValid() => MatchId > 0;
}