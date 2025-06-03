using Core.Constants;
using Core.Services;
using NSubstitute;
using RebateRetention.Constants;
using RebateRetention.Models;
using RebateRetention.Services.ProgramMessage;

namespace RebateRetention.UnitTests.Services.ProgramMessage;

public class DefaultTemplateProviderTests
{
    [Theory]
    [InlineData(Levels.Member, ContentLanguage.English, MessageContent.MemberEng)]
    [InlineData(Levels.Member, ContentLanguage.Cantonese, MessageContent.MemberCantonese)]
    [InlineData(Levels.Member, ContentLanguage.Mandarin, MessageContent.MemberMandarin)]
    [InlineData(Levels.Member, ContentLanguage.Thai, MessageContent.MemberThai)]
    [InlineData(Levels.Agent, ContentLanguage.English, MessageContent.SMAEng)]
    [InlineData(Levels.Agent, ContentLanguage.Cantonese, MessageContent.SMACantonese)]
    [InlineData(Levels.Agent, ContentLanguage.Mandarin, MessageContent.SMAMandarin)]
    [InlineData(Levels.Agent, ContentLanguage.Thai, MessageContent.SMAThai)]
    public void GetTemplate_ShouldReplaceProgramName(string level, string languageCode, string expectedTemplate)
    {
        // Arrange
        var refDataService = Substitute.For<RefDataService>();
        var ticket = new SabaWalletTicket { ProgramId = 1 };
        const string programName = "Test Program";
        refDataService.GetProgramName(ticket.ProgramId, languageCode).Returns(programName);

        var provider = new DefaultTemplateProvider(refDataService);

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        Assert.Equal(expectedTemplate.Replace("@program_name", programName), result);
    }

    [Fact]
    public void GetTemplate_ShouldReturnEmptyString_WhenLevelInvalid()
    {
        // Arrange
        var refDataService = Substitute.For<RefDataService>();
        var ticket = new SabaWalletTicket { ProgramId = 1 };
        var provider = new DefaultTemplateProvider(refDataService);

        // Act
        var result = provider.GetTemplate("InvalidLevel", ContentLanguage.English, ticket);

        // Assert
        Assert.Equal(string.Empty, result);
    }
}