using Fanex.Data.Repository;
using RebateRetention.Criteria;
using RebateRetention.Models;

namespace RebateRetention.Services;

public interface SabaWalletService
{
	Task<IEnumerable<SabaWalletTicket>> GetSabaWalletTickets();
}

public class SabaWalletServiceImp : SabaWalletService
{
	private readonly IDynamicRepository repository;

	public SabaWalletServiceImp(IDynamicRepository repository)
	{
		this.repository = repository;
	}

	public async Task<IEnumerable<SabaWalletTicket>> GetSabaWalletTickets()
	{
		var criteria = new SabaWalletTicketSelCriteria();

		return await repository.FetchAsync<SabaWalletTicket>(criteria);
	}
}