using Fanex.Data.Repository;
using RebateRetention.Criteria;

namespace RebateRetention.Services;

internal interface SabaProgramStatusService
{
	Task UpdateSabaProgramStatus(SabaProgramStatusUpdCriteria criteria);
}

internal class SabaProgramStatusServiceImp : SabaProgramStatusService
{
	private readonly IDynamicRepository repository;

	public SabaProgramStatusServiceImp(IDynamicRepository repository)
	{
		this.repository = repository;
	}

	public async Task UpdateSabaProgramStatus(SabaProgramStatusUpdCriteria criteria)
	{
		await repository.ExecuteAsync(criteria);
	}
}