using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.Data;
using Fanex.Data.Repository;
using Microsoft.VisualBasic;

namespace Core.Criteria;

public class MsgInsCriteria : NonQueryCommand
{
	public int? MessageId { get; set; }

	[SpParam(SqlParamName = "Content_en")]
	public string ContentEng { get; set; } = string.Empty;

	[SpParam(SqlParamName = "Content_ch")]
	public string ContentCh { get; set; } = string.Empty;

	[SpParam(SqlParamName = "Content_cs")]
	public string ContentCs { get; set; } = string.Empty;

	[SpParam(SqlParamName = "Content_id")]
	public string ContentId { get; set; } = string.Empty;

	public int? MatchId { get; set; }

	public short MsgType { get; set; }

	public int MsgContentsType { get; set; }

	public int Creator { get; set; }

	public string CreatorName { get; set; } = string.Empty;

	public override string GetSettingKey() => "NAP_SpecialDelete_Message_Insert";

	public override bool IsValid() => !string.IsNullOrEmpty(ContentEng);
}