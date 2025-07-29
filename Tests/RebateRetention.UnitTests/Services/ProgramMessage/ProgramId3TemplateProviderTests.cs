using Core.Constants;
using Core.Services;
using NSubstitute;
using RebateRetention.Constants;
using RebateRetention.Models;
using RebateRetention.Services.ProgramMessage;

namespace RebateRetention.UnitTests.Services.ProgramMessage;

public class ProgramId3TemplateProviderTests
{
    [Theory]
    [InlineData(Levels.Member, ContentLanguage.English, SixScoresContent.MemberEng, "USD", 1234.56, "USD 1,234.56")]
    [InlineData(Levels.Member, ContentLanguage.Cantonese, SixScoresContent.MemberCantonese, "VND", 1000000, "VND 1,000,000")]
    [InlineData(Levels.Member, ContentLanguage.Mandarin, SixScoresContent.MemberMandarin, "USD", 1234.56, "USD 1,234.56")]
    [InlineData(Levels.Member, ContentLanguage.Thai, SixScoresContent.MemberThai, "VND", 1000000, "VND 1,000,000")]
    public void GetTemplate_LevelsMember_ShouldReplaceProgramNameAndAmount(string level, string languageCode, string template, string currency, double amount, string expectedAmount)
    {
        // Arrange
        var refDataService = Substitute.For<RefDataService>();
        var ticket = new SabaWalletTicket { ProgramId = 3, Amount = (decimal)amount, CurrencyCode = currency };
        const string programName = "Test Program";
        refDataService.GetProgramName(ticket.ProgramId, languageCode).Returns(programName);

        var provider = new ProgramId3TemplateProvider(refDataService);

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        var expected = template.Replace("@program_name", programName).Replace("@amount", expectedAmount);
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(Levels.Agent, ContentLanguage.English, SixScoresContent.SMAEng, "USD", 1234.56, "USD 1,234.56")]
    [InlineData(Levels.Agent, ContentLanguage.Cantonese, SixScoresContent.SMACantonese, "VND", 1000000, "VND 1,000,000")]
    [InlineData(Levels.Agent, ContentLanguage.Mandarin, SixScoresContent.SMAMandarin, "USD", 1234.56, "USD 1,234.56")]
    [InlineData(Levels.Agent, ContentLanguage.Thai, SixScoresContent.SMAThai, "VND", 1000000, "VND 1,000,000")]
    public void GetTemplate_LevelsAgent_ShouldReplaceProgramNameAndAmount(string level, string languageCode, string template, string currency, double amount, string expectedAmount)
    {
        // Arrange
        var refDataService = Substitute.For<RefDataService>();
        var ticket = new SabaWalletTicket { ProgramId = 3, Amount = (decimal)amount, CurrencyCode = currency };
        const string programName = "Test Program";
        refDataService.GetProgramName(ticket.ProgramId, languageCode).Returns(programName);

        var provider = new ProgramId3TemplateProvider(refDataService);

        // Act
        var result = provider.GetTemplate(level, languageCode, ticket);

        // Assert
        var expected = template.Replace("@program_name", programName).Replace("@amount", expectedAmount);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetTemplate_ShouldReturnEmptyString_WhenLevelInvalid()
    {
        // Arrange
        var refDataService = Substitute.For<RefDataService>();
        var ticket = new SabaWalletTicket { ProgramId = 3, Amount = 100, CurrencyCode = "USD" };
        var provider = new ProgramId3TemplateProvider(refDataService);

        // Act
        var result = provider.GetTemplate("InvalidLevel", ContentLanguage.English, ticket);

        // Assert
        Assert.Equal(string.Empty, result);
    }
}