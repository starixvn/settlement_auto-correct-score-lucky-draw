namespace RebateRetention.Services;

public interface AutoRebateRetentionController
{
	Task Process();
}

public class AutoRebateRetentionControllerImp : AutoRebateRetentionController
{
	private readonly SabaWalletService sabaWalletService;

	public AutoRebateRetentionControllerImp(SabaWalletService sabaWalletService)
	{
		this.sabaWalletService = sabaWalletService;
	}

	public async Task Process()
	{
		var tickets = (await sabaWalletService.GetSabaWalletTickets()).ToList();

		foreach (var ticket in tickets)
		{
			// Process ticket
		}
	}
}