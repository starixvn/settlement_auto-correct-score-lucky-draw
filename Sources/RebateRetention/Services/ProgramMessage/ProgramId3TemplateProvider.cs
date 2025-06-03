using Core.Constants;
using Core.Services;
using RebateRetention.Constants;
using RebateRetention.Models;

namespace RebateRetention.Services.ProgramMessage;

public class ProgramId3TemplateProvider(RefDataService refDataService) : IMessageTemplateProvider
{
    public string GetTemplate(string level, string languageCode, SabaWalletTicket ticket)
    {
        var programName = refDataService.GetProgramName(ticket.ProgramId, languageCode);
        var template = level switch
        {
            Levels.Member => GetMemberTemplate(languageCode, ticket),
            Levels.Agent => GetAgentTemplate(languageCode),
            _ => string.Empty
        };

        return template.Replace("@program_name", programName);
    }

    private static string GetMemberTemplate(string languageCode, SabaWalletTicket ticket)
    {
        var template = languageCode switch
        {
            ContentLanguage.English => SixScoresContent.MemberEng,
            ContentLanguage.Cantonese => SixScoresContent.MemberCantonese,
            ContentLanguage.Mandarin => SixScoresContent.MemberMandarin,
            ContentLanguage.Thai => SixScoresContent.MemberThai,
            _ => string.Empty
        };

        return template.Replace("@amount", $"{ticket.CurrencyCode} {ticket.Amount:#,0.##}");
    }

    private static string GetAgentTemplate(string languageCode)
    {
        return languageCode switch
        {
            ContentLanguage.English => SixScoresContent.SMAEng,
            ContentLanguage.Cantonese => SixScoresContent.SMACantonese,
            ContentLanguage.Mandarin => SixScoresContent.SMAMandarin,
            ContentLanguage.Thai => SixScoresContent.SMAThai,
            _ => string.Empty
        };
    }
}