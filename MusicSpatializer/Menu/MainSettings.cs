using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;

namespace MusicSpatializer.Menu;

internal class MainSettings
{
    private readonly PluginConfig config;
    
    public MainSettings(PluginConfig config)
    {
        this.config = config;
    }
    
    [UIParams]
    private readonly BSMLParserParams parserParams = null!;
    
    public bool Enabled
    {
        get => config.enabled;
        set => config.enabled = value;
    }

    public bool Enable360 
    {
        get => config.enable360;
        set => config.enable360 = value;
    }

    public bool EnableResonance
    {
        get => config.enableResonance;
        set => config.enableResonance = value;
    }

    public bool EnableBassBoost 
    {
        get => config.enableBassBoost;
        set => config.enableBassBoost = value;
    }

    public bool EnableSpatialize 
    {
        get => config.enableSpatialize;
        set => config.enableSpatialize = value;
    }

    public float MusicVolumeMultiplier 
    {
        get => config.musicVolumeMultiplier;
        set => config.musicVolumeMultiplier = value;
    }

    public bool DebugSpheres 
    {
        get => config.debugSpheres;
        set => config.debugSpheres = value;
    }
    
    public void Apply()
    {
        config.Changed();
    }

    public void Ok()
    {
        config.Changed();
    }

    public void Cancel()
    {
        parserParams.EmitEvent("refresh-musicspatializer-values");
    }
}