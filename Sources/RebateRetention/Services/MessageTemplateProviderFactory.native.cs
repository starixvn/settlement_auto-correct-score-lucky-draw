namespace RebateRetention.Services;

using Core.Services;
using ProgramMessage;

public interface MessageTemplateProviderFactory
{
    IMessageTemplateProvider GetProvider(int programId);
}

public class MessageTemplateProviderFactoryImp : MessageTemplateProviderFactory
{
    private readonly RefDataService refDataService;
    private readonly Dictionary<int, IMessageTemplateProvider> templateProviders;

    public MessageTemplateProviderFactoryImp(RefDataService refDataService)
    {
        this.refDataService = refDataService;
        templateProviders = new Dictionary<int, IMessageTemplateProvider>
        {
            { 3, new ProgramId3TemplateProvider(this.refDataService) }
        };
    }

    public IMessageTemplateProvider GetProvider(int programId)
    {
        return templateProviders.TryGetValue(programId, out var provider) ? provider : new DefaultTemplateProvider(refDataService);
    }
}