using RebateRetention.Models;

namespace RebateRetention.Services.ProgramMessage;

public interface IMessageTemplateProvider
{
    string GetTemplate(string level, string languageCode, SabaWalletTicket ticket);
}