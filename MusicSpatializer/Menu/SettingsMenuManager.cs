using System;
using BeatSaberMarkupLanguage.Settings;
using Zenject;

namespace MusicSpatializer.Menu;

internal class SettingsMenuManager : IInitializable, IDisposable
{
    private readonly BSMLSettings bsmlSettings;
    private readonly MainSettings settingsMenu;
    
    public SettingsMenuManager(BSMLSettings bsmlSettings, MainSettings settingsMenu)
    {
        this.bsmlSettings = bsmlSettings;
        this.settingsMenu = settingsMenu;
    }

    public void Initialize()
    {
        bsmlSettings.AddSettingsMenu(
            "Music Spatializer",
            "MusicSpatializer.Menu.MainSettings.bsml",
            settingsMenu);
    }

    public void Dispose()
    {
        bsmlSettings.RemoveSettingsMenu(settingsMenu);
    }
}