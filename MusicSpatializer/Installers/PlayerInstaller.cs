using MusicSpatializer.Services;
using Zenject;

namespace MusicSpatializer.Installers;

internal class PlayerInstaller : Installer
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<SpatializerPlayerInitializer>().AsSingle();
    }
}