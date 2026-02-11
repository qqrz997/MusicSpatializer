using IPA;
using IPA.Config;
using IPA.Config.Stores;
using MusicSpatializer.Installers;
using SiraUtil.Zenject;
using Logger = IPA.Logging.Logger;

namespace MusicSpatializer;

[Plugin(RuntimeOptions.SingleStartInit)]
internal class Plugin
{
    public static Logger Log { get; private set; } = null!;

    [Init]
    public Plugin(Logger logger, Config config, Zenjector zenjector)
    {
        Log = logger;
        zenjector.UseLogger(logger);
        zenjector.Install<AppInstaller>(Location.App, config.Generated<PluginConfig>());
        zenjector.Install<MenuInstaller>(Location.Menu);
        zenjector.Install<PlayerInstaller>(Location.Player);
    }
}