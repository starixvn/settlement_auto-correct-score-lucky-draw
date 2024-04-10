using CorrectScoreLuckyDraw.Services;
using Fanex.Logging;
using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AutoCorrectScoreLuckyDraw.Service.Services;

public class LuckyDrawService : IHostedService
{
    private readonly AutoMatchController autoMatchController;

    private readonly AutoNotificationController autoNotificationController;

    private readonly IMaintenanceService maintenanceService;

    private readonly IConfiguration configuration;

    private bool running;

    private int counter;

    public LuckyDrawService(
        AutoMatchController autoMatchController,
        AutoNotificationController autoNotificationController,
        IConfiguration configuration,
        IMaintenanceService maintenanceService)
    {
        this.autoMatchController = autoMatchController;
        this.autoNotificationController = autoNotificationController;
        this.configuration = configuration;
        this.maintenanceService = maintenanceService;
        InitializeExtraComponent();
    }

    private static void InitializeExtraComponent()
    {
        TaskScheduler.UnobservedTaskException += TaskSchedulerUnobservedTaskException;
    }

    private static void TaskSchedulerUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
    {
        e.SetObserved();
        e.Exception.Handle(exception =>
        {
            var logMessage = new LogMessage(exception.Message)
                .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
                .ToString();
            LogService.Error(logMessage);
            return true;
        });
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        running = true;
        var autoRefundThread = new Thread(ThreadProcess);
        autoRefundThread.Start();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        running = false;
        return Task.CompletedTask;
    }

    private void ThreadProcess()
    {
        while (running)
        {
            if (!maintenanceService.IsMaintenance)
            {
                MainProcess();

                var interval = configuration.GetSection("ServiceInterval").Value ?? "0";

                Thread.Sleep(int.Parse(interval));
            }
        }
    }

    private void MainProcess()
    {
        try
        {
            //autoMatchController.Process();
            autoNotificationController.Process();
            HandleGarbageCollector();
        }
        catch (Exception exception)
        {
            LogService.Error("Error from Auto Correct Score Lucky Draw  Service(Soccer):", exception);
        }
    }

    private void HandleGarbageCollector()
    {
        var gcInterval = configuration.GetSection("GarbageCollectorInterval").Value ?? "0";
        if (counter >= int.Parse(gcInterval))
        {
            counter = 0;
            GC.WaitForPendingFinalizers();
        }

        var interval = configuration.GetSection("ServiceInterval").Value ?? "0";

        counter += int.Parse(interval);
    }
}