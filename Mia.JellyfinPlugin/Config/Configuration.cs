using MediaBrowser.Model.Plugins;

namespace Mia.Plugin;

public class Configuration : BasePluginConfiguration
{

    public Configuration()
    {
        BaseUrl = "";
        Token = "";
        SourceName = "Jellyfin";
        SourceUrl = "";
        SourceType = "jellyfin";
    }

    public string BaseUrl { get; set; }

    public string Token { get; set; }

    public string SourceName { get; set; }

    public string SourceUrl { get; set; }

    public string SourceType { get; set; }
}