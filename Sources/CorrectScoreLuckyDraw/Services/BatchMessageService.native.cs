using Core.Models;
using CorrectScoreLuckyDraw.Criteria;
using CorrectScoreLuckyDraw.Models;
using Fanex.Settlement.Converter.Services;
using MessageItem = CorrectScoreLuckyDraw.Models.MessageItem;

namespace CorrectScoreLuckyDraw.Services;

public interface BatchMessageService
{
	Task<int> GetMessageId(Ticket ticket, string level);

	Task<Result> UploadBatchMessage(List<MessageItem> items);
}

public class BatchMessageServiceImp(
	IDynamicRepository repository,
	MessageAnnouncementService messageAnnouncementService,
	XmlConvertService xmlConvertService) : BatchMessageService
{
	public async Task<int> GetMessageId(Ticket ticket, string level)
	{
		var criteria = messageAnnouncementService.CreateMessageCriteria(ticket, level);
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