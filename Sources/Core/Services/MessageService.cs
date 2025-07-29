using Core.Criteria;
using Core.Models;
using Fanex.Data.Repository;
using Fanex.Settlement.Converter.Services;

namespace Core.Services;

public interface IMessageService
{
    Task<int> CreateMessage(InsertMessageCriteria criteria);

    Task<Result> UploadBatchMessage(List<MessageItem> items);
}

public class MessageService(IDynamicRepository repository, XmlConvertService xmlConvertService)
    : IMessageService
{
    public async Task<int> CreateMessage(InsertMessageCriteria criteria)
    {
        return await repository.GetAsync<int>(criteria);
    }

    public async Task<Result> UploadBatchMessage(List<MessageItem> items)
    {
        var message = new Message
        {
            Items = items,
        };
        var xmlMessage = xmlConvertService.Serialize(message);

        var criteria = new UploadBatchMessageCriteria
        {
            XMLMessage = xmlMessage,
        };

        await repository.ExecuteAsync(criteria);

        return new Result
        {
            ErrorCode = criteria.ErrorCode,
            ErrorMsg = criteria.ErrorMsg
        };
    }
}