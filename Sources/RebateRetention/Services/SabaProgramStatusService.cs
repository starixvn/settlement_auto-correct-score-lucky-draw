using Fanex.Data.Repository;
using RebateRetention.Criteria;

namespace RebateRetention.Services;

public interface SabaProgramStatusService
{
    Task UpdateSabaProgramStatus(short sId);
}

public class SabaProgramStatusServiceImp(IDynamicRepository repository) : SabaProgramStatusService
{
    public async Task UpdateSabaProgramStatus(short sId)
    {
        var criteria = new SabaProgramStatusUpdCriteria
        {
            SId = sId,
            IsNotified = true
        };

        await repository.ExecuteAsync(criteria);
    }
}