namespace CorrectScoreLuckyDraw.Models;

public class AppSettingsKey
{
    public int RetryCount { get; set; }

    public int RetryInterval { get; set; }

    public Email RecipientEmail { get; set; }

    public Email RecipientEmailTest { get; set; }
}