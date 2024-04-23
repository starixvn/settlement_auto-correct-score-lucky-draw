using CorrectScoreLuckyDraw.Constants;
using CorrectScoreLuckyDraw.Criteria;
using CorrectScoreLuckyDraw.Models;

namespace CorrectScoreLuckyDraw.Services;

public interface MessageAnnouncementService
{
    InsertMessageCriteria CreateMessageCriteria(Ticket ticket, string level);

    MessageItem CreateMessageItem(Ticket ticket, int messageId, string level);
}

public class MessageAnnouncementServiceImp(RefDataService refDataService) : MessageAnnouncementService
{
    public InsertMessageCriteria CreateMessageCriteria(Ticket ticket, string level)
    {
        ticket.RefDataService = refDataService;
        if (!ticket.Site.Equals(Site.IBCIndo))
        {
            ticket.ExpiredDate = ticket.ExpiredDate.AddDays(1);
        }

        return new InsertMessageCriteria
        {
            ContentEng = CreateMessageContent(ticket, ContentLanguage.English, level),
            ContentCh = CreateMessageContent(ticket, ContentLanguage.Cantonese, level),
            ContentCs = CreateMessageContent(ticket, ContentLanguage.Mandarin, level),
            ContentId = CreateMessageContent(ticket, ContentLanguage.Indo, level),
            MatchId = ticket.MatchId,
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

    private static string GetTemplate(string level, string languageCode) => (level, languageCode) switch
    {
        (Levels.Member, ContentLanguage.English) => IbcIndoMember.ContentEng,
        (Levels.Member, ContentLanguage.Cantonese) => IbcIndoMember.ContentCantonese,
        (Levels.Member, ContentLanguage.Mandarin) => IbcIndoMember.ContentMandarin,
        (Levels.Member, ContentLanguage.Indo) => IbcIndoMember.ContentIndo,

        (Levels.Agent, ContentLanguage.English) or
        (Levels.Master, ContentLanguage.English) or
        (Levels.Super, ContentLanguage.English)
            => IbcIndoSMA.ContentEng,

        (Levels.Agent, ContentLanguage.Cantonese) or
        (Levels.Master, ContentLanguage.Cantonese) or
        (Levels.Super, ContentLanguage.Cantonese)
            => IbcIndoSMA.ContentCantonese,

        (Levels.Agent, ContentLanguage.Mandarin) or
        (Levels.Master, ContentLanguage.Mandarin) or
        (Levels.Super, ContentLanguage.Mandarin)
            => IbcIndoSMA.ContentMandarin,

        (Levels.Agent, ContentLanguage.Indo) or
        (Levels.Master, ContentLanguage.Indo) or
        (Levels.Super, ContentLanguage.Indo)
            => IbcIndoSMA.ContentIndo,

        _ => string.Empty,
    };

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