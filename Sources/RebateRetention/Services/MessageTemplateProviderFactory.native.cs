using Core.Constants;

namespace RebateRetention.Services;

using Core.Services;
using ProgramMessage;

public interface MessageTemplateProviderFactory
{
    IMessageTemplateProvider GetProvider(int programId);
}

public class MessageTemplateProviderFactoryImp(RefDataService refDataService) : MessageTemplateProviderFactory
{
    public IMessageTemplateProvider GetProvider(int programId)
    {
        var templateProviders = new Dictionary<int, IMessageTemplateProvider>
        {
            { ProgramId.SixScoreChallenge, new ProgramId3TemplateProvider(refDataService) }
        };

        return templateProviders.TryGetValue(programId, out var provider) ? provider : new DefaultTemplateProvider(refDataService);
    }
}