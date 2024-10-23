using Fanex.Data.Repository;
using NSubstitute;
using RebateRetention.Criteria;
using RebateRetention.Services;

namespace RebateRetention.UnitTests.Services;

public class SabaProgramStatusServiceTests
{
	private readonly IDynamicRepository _repository;
	private readonly SabaProgramStatusServiceImp _sabaProgramStatusService;

	public SabaProgramStatusServiceTests()
	{
		_repository = Substitute.For<IDynamicRepository>();
		_sabaProgramStatusService = new SabaProgramStatusServiceImp(_repository);
	}

	[Fact]
	public async Task UpdateSabaProgramStatus_ShouldCallRepositoryWithCorrectCriteria()
	{
		// Arrange
		short sId = 123;
		var expectedCriteria = new SabaProgramStatusUpdCriteria
		{
			SId = sId,
			IsNotified = true
		};

		// Act
		await _sabaProgramStatusService.UpdateSabaProgramStatus(sId);

		// Assert
		await _repository.Received(1).ExecuteAsync(Arg.Is<SabaProgramStatusUpdCriteria>(criteria =>
			criteria.SId == expectedCriteria.SId &&
			criteria.IsNotified == expectedCriteria.IsNotified
		));
	}
}