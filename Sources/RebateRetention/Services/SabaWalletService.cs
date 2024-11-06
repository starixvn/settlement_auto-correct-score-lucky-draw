using Core.Models;
using Fanex.Data.Repository;
using Microsoft.Extensions.Options;
using RebateRetention.Criteria;
using RebateRetention.Models;

namespace RebateRetention.Services;

public interface SabaWalletService
{
    Task<IEnumerable<SabaWalletTicket>> GetSabaWalletTickets();
}

public class SabaWalletServiceImp(IDynamicRepository repository, IOptions<AppSettingsKey> options) : SabaWalletService
{
    public async Task<IEnumerable<SabaWalletTicket>> GetSabaWalletTickets()
    {
        var isTest = options.Value.EnvironmentName == "UAT";
        var criteria = new SabaWalletTicketSelCriteria 
        {
            IsTest = isTest
        };

        return await repository.FetchAsync<SabaWalletTicket>(criteria);
    }
}