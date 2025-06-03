using Core.Constants;
using Core.Services;
using RebateRetention.Constants;
using RebateRetention.Models;

namespace RebateRetention.Services.ProgramMessage;

public class DefaultTemplateProvider(RefDataService refDataService) : IMessageTemplateProvider
{
    public string GetTemplate(string level, string languageCode, SabaWalletTicket ticket)
    {
        var programName = refDataService.GetProgramName(ticket.ProgramId, languageCode);
        var template = level switch
        {
            Levels.Member => GetMemberTemplate(languageCode),
            Levels.Agent => GetAgentTemplate(languageCode),
            _ => string.Empty
        };

        return template.Replace("@program_name", programName);
    }

    private static string GetMemberTemplate(string languageCode)
    {
        return languageCode switch
        {
            ContentLanguage.English => MessageContent.MemberEng,
            ContentLanguage.Cantonese => MessageContent.MemberCantonese,
            ContentLanguage.Mandarin => MessageContent.MemberMandarin,
            ContentLanguage.Thai => MessageContent.MemberThai,
            _ => string.Empty
        };
    }

    private static string GetAgentTemplate(string languageCode)
    {
        return languageCode switch
        {
            ContentLanguage.English => MessageContent.SMAEng,
            ContentLanguage.Cantonese => MessageContent.SMACantonese,
            ContentLanguage.Mandarin => MessageContent.SMAMandarin,
            ContentLanguage.Thai => MessageContent.SMAThai,
            _ => string.Empty
        };
    }
}