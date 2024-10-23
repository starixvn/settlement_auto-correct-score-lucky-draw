using Fanex.Data.Repository;
using RebateRetention.Criteria;
using RebateRetention.Models;

namespace RebateRetention.Services;

public interface SabaWalletService
{
    Task<IEnumerable<SabaWalletTicket>> GetSabaWalletTickets();
}

public class SabaWalletServiceImp(IDynamicRepository repository) : SabaWalletService
{
    public async Task<IEnumerable<SabaWalletTicket>> GetSabaWalletTickets()
    {
        var criteria = new SabaWalletTicketSelCriteria();

        return await repository.FetchAsync<SabaWalletTicket>(criteria);
    }
}