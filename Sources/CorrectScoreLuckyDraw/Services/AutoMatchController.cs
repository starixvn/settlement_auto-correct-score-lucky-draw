using CorrectScoreLuckyDraw.Models;
using Fanex.Settlement.Converter.Services;

namespace CorrectScoreLuckyDraw.Services;

public interface AutoMatchController
{
    Task Process();
}

public class AutoMatchControllerImp(
    MatchService matchService,
    MemberService memberService,
    SettleLogService logService,
    XmlConvertService xmlConvertService) : AutoMatchController
{
    public async Task Process()
    {
        var matches = await matchService.GetMatch();
        foreach (var match in matches)
        {
            var members = await memberService.GetMembersByMatchId(match);
            if (members.Count > 0)
            {
                var xmlMembers = GenerateXmlMembers(members);
                await memberService.InsertMembers(match, xmlMembers);
                await WriteLog(match, xmlMembers);
            }
        }
    }

    private string GenerateXmlMembers(List<Member> members)
    {
        var luckyDrawMember = new LuckyDrawMember
        {
            Members = members
        };

        return xmlConvertService.Serialize(luckyDrawMember);
    }

    private async Task WriteLog(Match match, string members)
    {
        var remark = $"MinBet: {match.MinBet}, MinPool: {match.MinPool}, MaxPool: {match.MaxPool}, " +
                     $"TotalPool: {match.TotalPool}, TotalWinner: {match.TotalWinner}.";

        await logService.Log(match.MatchId, remark);
        await logService.Log(match.MatchId, members);
    }
}