using System;

namespace Core.Models;

public class AppSettingsKey
{
    public Email RecipientEmail { get; set; }

    public Email RecipientEmailTest { get; set; }

    public string EnvironmentName { get; set; }
}
