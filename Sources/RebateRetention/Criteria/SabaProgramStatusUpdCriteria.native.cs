using Fanex.Data.Repository;

namespace RebateRetention.Criteria;

public class SabaProgramStatusUpdCriteria : INonQueryCommand
{
    public short SId { get; set; }
    public bool IsNotified { get; set; }

    public string GetSettingKey() => "NAP_SabaProgramStatus_Upd";

    public bool IsValid() => SId > 0;
}