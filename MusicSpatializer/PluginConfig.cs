using System.Runtime.CompilerServices;
using IPA.Config.Stores;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace MusicSpatializer;

internal class PluginConfig
{
    public bool enabled { get; set; } = true;
    public bool enable360 { get; set; } = true;
    public bool enableResonance { get; set; } = true;
    public bool enableBassBoost { get; set; } = false;
    public bool enableSpatialize { get; set; } = true;
    public float musicVolumeMultiplier { get; set; } = 1.0f;
    public bool debugSpheres { get; set; } = false;

    public virtual void Changed() { }

    public virtual void OnReload() { }
}