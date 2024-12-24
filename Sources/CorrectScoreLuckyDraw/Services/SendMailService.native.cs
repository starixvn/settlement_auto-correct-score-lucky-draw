using Core.Models;
using CorrectScoreLuckyDraw.Models;
using Fanex.MailDeliver.MailClient.Model;
using Microsoft.Extensions.Options;
using RazorEngineCore;
using System.Collections.Concurrent;

namespace CorrectScoreLuckyDraw.Services;

public interface SendMailService
{
    void SendMail(NotificationModel item);
}

public class SendMailServiceImp(IOptions<AppSettingsKey> appSettingsKey) : SendMailService
{
    private static readonly ConcurrentDictionary<int, IRazorEngineCompiledTemplate> CompiledTemplateCache = new();

    public void SendMail(NotificationModel item)
    {
        string mailTo;
        string mailCc;
        if (item.Match.IsTest)
        {
            mailTo = appSettingsKey.Value.RecipientEmailTest.To;
            mailCc = appSettingsKey.Value.RecipientEmailTest.CC;
        }
        else
        {
            mailTo = appSettingsKey.Value.RecipientEmail.To;
            mailCc = appSettingsKey.Value.RecipientEmail.CC;
        }

        var mailRecord = new MailRecord
        {
            MailTo = mailTo,
            MailCc = mailCc,
            Title = "NAP - Settlement System",
            Subject = Subject(item.Match),
            Body = MailBody(item)
        };

        using var mailClient = new Fanex.MailDeliver.MailClient.API.MailDeliver();

        mailClient.SendMail(mailRecord);
    }

    private static string MailBody(NotificationModel item)
    {
        var templatePath = Path.Combine("Templates", "template.cshtml");

        var hashCode = templatePath.GetHashCode();
        var compiledTemplate = CompiledTemplateCache.GetOrAdd(hashCode, _ =>
        {
            var template = File.ReadAllText(templatePath);
            var razorEngine = new RazorEngine();
            return razorEngine.Compile(template);
        });

        return compiledTemplate.Run(item);
    }

    private static string Subject(Match match)
    {
        return $"Correct Score Lucky Draw Winning List - Match ID: {match.MatchId} ({match.TeamName})";
    }
}