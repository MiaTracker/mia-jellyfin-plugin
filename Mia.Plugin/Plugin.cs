using System.Globalization;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace Mia.Plugin;

public class Plugin : BasePlugin<Configuration>, IHasWebPages
{

    public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer) : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
    }

    public override string Name => "Mia";

    public override Guid Id => Guid.Parse("90a6d753-8f88-4be9-805c-9df49ecd0afb");

    public static Plugin? Instance { get; private set; }

    public IEnumerable<PluginPageInfo> GetPages()
    {
        return new[]
        {
            new PluginPageInfo()
            {
                Name = this.Name,
                EmbeddedResourcePath = string.Format(CultureInfo.InvariantCulture, "{0}.Config.configPage.html",
                    GetType().Namespace)
            }
        };
    }
}