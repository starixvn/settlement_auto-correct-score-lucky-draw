using Fanex.Data.Repository;

namespace RebateRetention.Criteria;

internal class SabaWalletTicketSelCriteria : CriteriaBase
{
	public override string GetSettingKey() => "NAP_SabaWalletTicket_Notification_Sel";

	public override bool IsValid() => true;
}