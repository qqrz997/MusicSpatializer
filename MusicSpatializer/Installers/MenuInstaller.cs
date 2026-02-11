using MusicSpatializer.Menu;
using MusicSpatializer.Services;
using Zenject;

namespace MusicSpatializer.Installers;

internal class MenuInstaller : Installer
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<SettingsMenuManager>().AsSingle();
        Container.Bind<MainSettings>().AsSingle();
        Container.BindInterfacesTo<SpatializerInitializer>().AsSingle();
    }
}