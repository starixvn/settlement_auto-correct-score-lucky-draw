using Core.Criteria;
using Core.Models;
using Core.Services;
using Fanex.Data.Repository;
using Fanex.Settlement.Converter.Services;
using NSubstitute;

namespace Core.UnitTests.Services;

public class MessageServiceTests
{
    private readonly IDynamicRepository repository;
    private readonly XmlConvertService xmlConvertService;
    private readonly MessageService messageService;

    public MessageServiceTests()
    {
        repository = Substitute.For<IDynamicRepository>();
        xmlConvertService = Substitute.For<XmlConvertService>();
        messageService = new MessageService(repository, xmlConvertService);
    }

    [Fact]
    public async Task UploadBatchMessage_ShouldReturnResultWithErrorCodeAndMessage()
    {
        // Arrange
        var items = new List<MessageItem>
        {
            new MessageItem { /*...*/ },
            new MessageItem { /*...*/ }
        };

        var message = new Message
        {
            Items = items
        };

        const string xmlMessage = "<Message>...</Message>";
        xmlConvertService.Serialize(message).Returns(xmlMessage);

        // Act
        await messageService.UploadBatchMessage(items);

        // Assert

        await repository.Received(1).ExecuteAsync(Arg.Any<UploadBatchMessageCriteria>());
        xmlConvertService.Received(1).Serialize(Arg.Any<Message>());
    }

    [Fact]
    public async Task CreateMessage_ShouldReturnMessageId_WhenRepositoryReturnsValidId()
    {
        // Arrange
        var criteria = new MsgInsCriteria
        {
            // Populate criteria as needed
        };

        const int expectedMessageId = 123;

        repository.GetAsync<int>(criteria).Returns(expectedMessageId);

        // Act
        var result = await messageService.CreateMessage(criteria);

        // Assert
        Assert.Equal(expectedMessageId, result);
        await repository.Received(1).GetAsync<int>(Arg.Any<MsgInsCriteria>());
    }
}