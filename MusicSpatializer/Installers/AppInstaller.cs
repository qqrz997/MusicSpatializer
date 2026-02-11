using Zenject;

namespace MusicSpatializer.Installers;

internal class AppInstaller : Installer
{
    private readonly PluginConfig config;

    public AppInstaller(PluginConfig config)
    {
        this.config = config;
    }

    public override void InstallBindings()
    {
        Container.BindInstance(config).AsSingle();
    }
}