using MusicSpatializer.Services;
using Zenject;

namespace MusicSpatializer.Installers;

public class PlayerInstaller : Installer
{
    public override void InstallBindings()
    {
        Container.BindInterfacesTo<SpatializerInitializer>().AsSingle();
    }
}