using CorrectScoreLuckyDraw.Models;
using CorrectScoreLuckyDraw.Services;
using Fanex.Settlement.Converter.Services;
using NSubstitute;

namespace CorrectScoreLuckyDraw.UnitTests;

public class AutoMatchControllerTests
{
    private readonly MatchService matchService;

    private readonly MemberService memberService;

    private readonly SettleLogService settleLogService;

    private readonly XmlConvertService xmlConvertService;

    private readonly AutoMatchController service;

    public AutoMatchControllerTests()
    {
        matchService = Substitute.For<MatchService>();
        memberService = Substitute.For<MemberService>();
        settleLogService = Substitute.For<SettleLogService>();
        xmlConvertService = Substitute.For<XmlConvertService>();
        service = new AutoMatchControllerImp(matchService, memberService, settleLogService, xmlConvertService);
    }

    [Fact]
    public async Task Process_IsNotExistedMatch_ReturnsNotCallAnyMethod()
    {
        matchService.GetMatch().Returns([]);
        await service.Process();

        await memberService.DidNotReceive().GetMembersByMatchId(Arg.Any<Match>());
        await memberService.DidNotReceive().InsertMembers(Arg.Any<Match>(), Arg.Any<string>());
        xmlConvertService.DidNotReceive().Serialize(Arg.Any<LuckyDrawMember>());
    }

    [Fact]
    public async Task Process_IsExistedMatchAndNotExistedMembers_ReturnsNotCallInsertMembers()
    {
        var matches = new List<Match>
        {
            new()
            {
                MatchId = 1
            }
        };
        memberService.GetMembersByMatchId(Arg.Any<Match>()).Returns([]);
        matchService.GetMatch().Returns(matches);
        await service.Process();

        await memberService.DidNotReceive().InsertMembers(Arg.Any<Match>(), Arg.Any<string>());
        xmlConvertService.DidNotReceive().Serialize(Arg.Any<LuckyDrawMember>());
    }

    [Fact]
    public async Task Process_IsExistedMatchAndMembers_ReturnsCallExpectedMethod()
    {
        var matches = new List<Match>
        {
            new()
            {
                MatchId = 1
            }
        };
        var members = new List<Member>
        {
            new()
            {
                CustId = 1
            }
        };

        memberService.GetMembersByMatchId(Arg.Any<Match>()).Returns(members);
        matchService.GetMatch().Returns(matches);
        await service.Process();

        await memberService.Received().InsertMembers(Arg.Any<Match>(), Arg.Any<string>());
        xmlConvertService.Received().Serialize(Arg.Any<LuckyDrawMember>());
        await settleLogService.Received(2).Log(Arg.Any<int>(), Arg.Any<string>());
    }
}