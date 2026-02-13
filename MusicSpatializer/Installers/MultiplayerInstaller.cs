using MusicSpatializer.Services;
using Zenject;

namespace MusicSpatializer.Installers;

internal class MultiplayerInstaller : Installer
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<SpatializerMultiplayerInitializer>().AsSingle();
    }
}