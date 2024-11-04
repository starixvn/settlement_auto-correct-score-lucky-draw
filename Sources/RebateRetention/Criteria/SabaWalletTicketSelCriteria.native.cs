using Fanex.Data.Repository;

namespace RebateRetention.Criteria;

public class SabaWalletTicketSelCriteria : CriteriaBase
{
    public bool IsTest { get; set; } = false;
    public override string GetSettingKey() => "NAP_SabaWalletTicket_Notification_Sel";

    public override bool IsValid() => true;
}