using Core.Constants;
using Core.Criteria;
using Core.Models;
using Core.Services;
using CorrectScoreLuckyDraw.Constants;
using CorrectScoreLuckyDraw.Models;

namespace CorrectScoreLuckyDraw.Services;

public interface MessageAnnouncementService
{
    MsgInsCriteria CreateMessageCriteria(Ticket ticket, string level);

    MessageItem CreateMessageItem(Ticket ticket, int messageId, string level);
}

public class MessageAnnouncementServiceImp(RefDataService refDataService) : MessageAnnouncementService
{
    public MsgInsCriteria CreateMessageCriteria(Ticket ticket, string level)
    {
        ticket.RefDataService = refDataService;
        if (!ticket.Site.Equals(Site.IBCIndo))
        {
            ticket.ExpiredDate = ticket.ExpiredDate.AddDays(1);
        }

        return new MsgInsCriteria
        {
            ContentEng = CreateMessageContent(ticket, ContentLanguage.English, level),
            ContentCh = CreateMessageContent(ticket, ContentLanguage.Cantonese, level),
            ContentCs = CreateMessageContent(ticket, ContentLanguage.Mandarin, level),
            ContentId = CreateMessageContent(ticket, ContentLanguage.Indo, level),
            MatchId = ticket.MatchId,
            MsgType = 1,
            MsgContentsType = 112
        };
    }

    private static string CreateMessageContent(Ticket ticket, string languageCode, string level)
    {
        var template = string.IsNullOrEmpty(level) ? GetTemplate(languageCode) : GetTemplate(level, languageCode);
        var leagueName = GetLeagueName(ticket, languageCode);
        var teamName = GetTeamName(ticket, languageCode);

        return template
            .Replace("@team_vs_team", teamName)
            .Replace("@league_name", leagueName)
            .Replace("@email", "support@ibcbet.com")
            .Replace("@member_username", ticket.Username)
            .Replace("@currency_name", ticket.CurrencyName)
            .Replace("@prize_amount", ticket.PrizeAmount.ToString("#,0"))
            .Replace("@verification_code", ticket.VerificationCode)
            .Replace("@expired_date", ticket.ExpiredDate.ToString("dd/MM/yyyy"));
    }

    private static string GetTeamName(Ticket ticket, string languageCode) =>
        languageCode switch
        {
            ContentLanguage.Cantonese => ticket.TeamNameCh,
            ContentLanguage.Mandarin => ticket.TeamNameCs,
            ContentLanguage.Indo => ticket.TeamNameIndo,
            _ => ticket.TeamName,
        };

    private static string GetLeagueName(Ticket ticket, string languageCode) =>
        languageCode switch
        {
            ContentLanguage.Cantonese => ticket.LeagueNameCh,
            ContentLanguage.Mandarin => ticket.LeagueNameCs,
            ContentLanguage.Indo => ticket.LeagueNameIndo,
            _ => ticket.LeagueName,
        };

    private static string GetTemplate(string languageCode) => (languageCode) switch
    {
        ContentLanguage.English => NotIbcIndo.ContentEng,
        ContentLanguage.Cantonese => NotIbcIndo.ContentCantonese,
        ContentLanguage.Mandarin => NotIbcIndo.ContentMandarin,
        _ => string.Empty,
    };

    private static string GetTemplate(string level, string languageCode)
    {
        return level switch
        {
            Levels.Member => GetTemplateForMember(languageCode),
            Levels.Agent => GetTemplateForAgent(languageCode),
            Levels.Master => GetTemplateForMaster(languageCode),
            Levels.Super => GetTemplateForSuper(languageCode),
            _ => string.Empty
        };
    }

    private static string GetTemplateForMember(string languageCode)
    {
        return languageCode switch
        {
            ContentLanguage.English => IbcIndoMember.ContentEng,
            ContentLanguage.Cantonese => IbcIndoMember.ContentCantonese,
            ContentLanguage.Mandarin => IbcIndoMember.ContentMandarin,
            ContentLanguage.Indo => IbcIndoMember.ContentIndo,
            _ => string.Empty
        };
    }

    private static string GetTemplateForAgent(string languageCode)
    {
        return languageCode switch
        {
            ContentLanguage.English => IbcIndoSMA.ContentEng,
            ContentLanguage.Cantonese => IbcIndoSMA.ContentCantonese,
            ContentLanguage.Mandarin => IbcIndoSMA.ContentMandarin,
            ContentLanguage.Indo => IbcIndoSMA.ContentIndo,
            _ => string.Empty
        };
    }

    private static string GetTemplateForMaster(string languageCode)
    {
        return languageCode switch
        {
            ContentLanguage.English => IbcIndoSMA.ContentEng,
            ContentLanguage.Cantonese => IbcIndoSMA.ContentCantonese,
            ContentLanguage.Mandarin => IbcIndoSMA.ContentMandarin,
            ContentLanguage.Indo => IbcIndoSMA.ContentIndo,
            _ => string.Empty
        };
    }

    private static string GetTemplateForSuper(string languageCode)
    {
        return languageCode switch
        {
            ContentLanguage.English => IbcIndoSMA.ContentEng,
            ContentLanguage.Cantonese => IbcIndoSMA.ContentCantonese,
            ContentLanguage.Mandarin => IbcIndoSMA.ContentMandarin,
            ContentLanguage.Indo => IbcIndoSMA.ContentIndo,
            _ => string.Empty
        };
    }

    public MessageItem CreateMessageItem(Ticket ticket, int messageId, string level)
    {
        return level switch
        {
            Levels.Agent => new MessageItem
            {
                MessageId = messageId,
                TargetId = ticket.AgentId,
                TargetName = ticket.AgentUsername
            },

            Levels.Master => new MessageItem
            {
                MessageId = messageId,
                TargetId = ticket.MasterId,
                TargetName = ticket.MasterUsername
            },

            Levels.Super => new MessageItem
            {
                MessageId = messageId,
                TargetId = ticket.SuperId,
                TargetName = ticket.SuperUsername
            },

            _ => new MessageItem { MessageId = messageId, TargetId = ticket.CustId, TargetName = ticket.Username }
        };
    }
}