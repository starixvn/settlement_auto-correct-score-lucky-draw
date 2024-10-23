using Fanex.Data.Repository;
using RebateRetention.Criteria;

namespace RebateRetention.Services;

public interface SabaProgramStatusService
{
    Task UpdateSabaProgramStatus(int sId);
}

public class SabaProgramStatusServiceImp(IDynamicRepository repository) : SabaProgramStatusService
{
    public async Task UpdateSabaProgramStatus(int sId)
    {
        var criteria = new SabaProgramStatusUpdCriteria
        {
            SId = sId,
            IsNotified = true
        };

        await repository.ExecuteAsync(criteria);
    }
}