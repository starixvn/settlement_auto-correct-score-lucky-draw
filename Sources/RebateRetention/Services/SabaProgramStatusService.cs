using Fanex.Data.Repository;
using RebateRetention.Criteria;

namespace RebateRetention.Services;

public interface SabaProgramStatusService
{
	Task UpdateSabaProgramStatus(short sId);
}

public class SabaProgramStatusServiceImp : SabaProgramStatusService
{
	private readonly IDynamicRepository repository;

	public SabaProgramStatusServiceImp(IDynamicRepository repository)
	{
		this.repository = repository;
	}

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