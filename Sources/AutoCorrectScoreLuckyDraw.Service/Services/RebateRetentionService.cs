using Fanex.Logging.Extensions.Database;
using Fanex.Logging.Extensions;
using Fanex.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Core.Services;
using RebateRetention.Services;

namespace AutoCorrectScoreLuckyDraw.Service.Services;

public class RebateRetentionService : BackgroundService
{
	private readonly IConfiguration configuration;
	private readonly IMaintenanceService maintenanceService;
	private readonly AutoRebateRetentionController autoRebateRetentionController;

	public RebateRetentionService(
		IConfiguration configuration,
		IMaintenanceService maintenanceService,
		AutoRebateRetentionController autoRebateRetentionController)
	{
		this.configuration = configuration;
		this.maintenanceService = maintenanceService;
		this.autoRebateRetentionController = autoRebateRetentionController;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		using PeriodicTimer timer = new(TimeSpan.FromMilliseconds(configuration.GetValue<int>("ServiceInterval")));

		while (await timer.WaitForNextTickAsync(stoppingToken))
		{
			await HandleAutoAsync();
		}
	}

	private async Task HandleAutoAsync()
	{
		try
		{
			if (!maintenanceService.IsMaintenance)
			{
				await autoRebateRetentionController.Process();
				HandleGarbageCollector();
			}
		}
		catch (Exception exception)
		{
			var logMessage = new LogMessage(exception.Message)
				.AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
				.AddSection("Server Info", CommonInfoGetters.GetServerInfo())
				.AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
				.ToString();
			await LogService.ErrorAsync(logMessage);
		}
	}

	private void HandleGarbageCollector()
	{
		//if (counter >= configuration.GetValue<int>("GarbageCollectorInterval"))
		//{
		//	counter = 0;
		//	GC.WaitForPendingFinalizers();
		//}

		//counter += configuration.GetValue<int>("ServiceInterval");
	}
}