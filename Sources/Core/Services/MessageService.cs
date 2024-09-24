using Core.Criteria;
using Core.Models;
using Fanex.Data.Repository;
using Fanex.Settlement.Converter.Services;

namespace Core.Services;

public interface IMessageService
{
	Task<int> CreateMessage(MsgInsCriteria criteria);

	Task<Result> UploadBatchMessage(List<MessageItem> items);
}

public class MessageService : IMessageService
{
	private readonly IDynamicRepository repository;
	private readonly XmlConvertService xmlConvertService;

	public MessageService(IDynamicRepository repository, XmlConvertService xmlConvertService)
	{
		this.repository = repository;
		this.xmlConvertService = xmlConvertService;
	}

	public async Task<int> CreateMessage(MsgInsCriteria criteria)
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