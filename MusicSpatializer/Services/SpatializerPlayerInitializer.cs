using MusicSpatializer.Extensions;
using UnityEngine;
using Zenject;

namespace MusicSpatializer.Services;

internal class SpatializerPlayerInitializer : IInitializable
{
    private readonly PluginConfig config;
    private readonly SettingsManager settingsManager;
    private readonly SongController songController;
    
    public SpatializerPlayerInitializer(
        PluginConfig config,
        SettingsManager settingsManager,
        SongController songController)
    {
        this.config = config;
        this.settingsManager = settingsManager;
        this.songController = songController;
    }
    
    public void Initialize()
    {
        if (!config.enabled)
        {
            return;
        }

        if (songController.TryGetComponent<AudioSource>(out var songControllerAudioSource))
        {
            songControllerAudioSource.DecorateAudioSource(config, settingsManager);
            // songControllerAudioSource.gameObject.AddComponent<MainSongAudioSourceRestarterBugFix>();
        }
        else
        {
            Plugin.Log.Critical($"Unable to get {nameof(AudioSource)} from {nameof(songController)}");
        }
    }
}