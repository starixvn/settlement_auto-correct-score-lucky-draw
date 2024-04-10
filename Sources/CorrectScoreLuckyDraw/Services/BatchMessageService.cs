using CorrectScoreLuckyDraw.Criteria;
using CorrectScoreLuckyDraw.Models;
using Fanex.Settlement.Converter.Services;
using ThirdPartyBroadcast.Consumer.Criteria;

namespace CorrectScoreLuckyDraw.Services;

public interface BatchMessageService
{
    Task<int> GetMessageId(Ticket notification);

    Task<Result> CreateMessageUpload(List<MessageItem> items, int adminId);
}

public class BatchMessageServiceImp(
    IDynamicRepository repository,
    RefDataService refDataService,
    XmlConvertService xmlConvertService) : BatchMessageService
{
    private InsertMessageCriteria CreateMessageCriteria(Ticket notification)
    {
        notification.RefDataService = refDataService;
        return new InsertMessageCriteria();
    }

    public async Task<int> GetMessageId(Ticket notification)
    {
        var criteria = CreateMessageCriteria(notification);
        return await repository.GetAsync<int>(criteria);
    }

    public async Task<Result> CreateMessageUpload(List<MessageItem> items, int adminId)
    {
        var message = new Message
        {
            Items = items,
        };

        var xmlMessage = xmlConvertService.Serialize(message);

        var criteria = new UploadBatchMessageCriteria
        {
            XMLMessage = xmlMessage,
            CreatorId = adminId
        };

        await repository.ExecuteAsync(criteria);

        return new Result
        {
            ErrorCode = criteria.ErrorCode,
            ErrorMsg = criteria.ErrorMsg
        };
    }
}