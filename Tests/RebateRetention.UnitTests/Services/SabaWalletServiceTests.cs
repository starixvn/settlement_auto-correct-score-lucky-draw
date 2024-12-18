using Core.Models;
using Fanex.Data.Repository;
using Microsoft.Extensions.Options;
using NSubstitute;
using RebateRetention.Criteria;
using RebateRetention.Models;
using RebateRetention.Services;

namespace RebateRetention.UnitTests.Services;

public class SabaWalletServiceTests
{
    private readonly IDynamicRepository repository;
    private readonly SabaWalletServiceImp sabaWalletService;

    public SabaWalletServiceTests()
    {
        var appSettingsKey = Substitute.For<IOptions<AppSettingsKey>>();
        appSettingsKey.Value.Returns(new AppSettingsKey { EnvironmentName = "UAT" });
        repository = Substitute.For<IDynamicRepository>();
        sabaWalletService = new SabaWalletServiceImp(repository, appSettingsKey);
    }

    [Fact]
    public async Task GetSabaWalletTickets_ShouldReturnTicketsFromRepository()
    {
        // Arrange
        var expectedTickets = new List<SabaWalletTicket>
        {
            new() { ProgramId = 1, TransId = 12, CustId = 1001 },
            new() { ProgramId = 2, TransId = 123, CustId = 1002 }
        };

        repository.FetchAsync<SabaWalletTicket>(Arg.Any<SabaWalletTicketSelCriteria>())
                   .Returns(expectedTickets);

        // Act
        var result = await sabaWalletService.GetSabaWalletTickets();

        // Assert
        await repository.Received(1).FetchAsync<SabaWalletTicket>(Arg.Any<SabaWalletTicketSelCriteria>());
        Assert.Equal(expectedTickets, result);
    }
}