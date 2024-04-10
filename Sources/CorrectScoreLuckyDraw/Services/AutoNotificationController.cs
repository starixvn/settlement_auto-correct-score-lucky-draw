using CorrectScoreLuckyDraw.Models;
using Fanex.Settlement.Converter.Services;

namespace CorrectScoreLuckyDraw.Services;

public interface AutoNotificationController
{
    Task Process();
}

public class AutoNotificationControllerImp(
    IDynamicRepository repository,
    NotificationService notificationService,
    BatchMessageService messageService,
    XmlConvertService xmlConvertService) : AutoNotificationController
{
    public async Task Process()
    {
        var notifications = await notificationService.GetNotification();
        foreach (var notification in notifications)
        {
            var value = new Ticket();
            var messageId = await messageService.GetMessageId(value);
            if (messageId > 0)
            {
                var listMessage = new List<MessageItem>();
                var result = await messageService.CreateMessageUpload(listMessage, 1);
            }
        }

        //notificationService.UpdateNotification();
    }
}