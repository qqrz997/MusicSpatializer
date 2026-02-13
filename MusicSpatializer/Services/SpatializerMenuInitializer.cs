using System;
using System.Collections.Generic;
using System.Linq;
using BeatSaberMarkupLanguage;
using MusicSpatializer.Extensions;
using UnityEngine;
using Zenject;

namespace MusicSpatializer.Services;

internal class SpatializerMenuInitializer : IInitializable
{
    private readonly PluginConfig config;
    private readonly SettingsManager settingsManager;
    private readonly SongPreviewPlayer songPreviewPlayer;
    private readonly CenterStageScreenController centerStageScreenController;

    public SpatializerMenuInitializer(PluginConfig config,
        SettingsManager settingsManager,
        SongPreviewPlayer songPreviewPlayer, 
        CenterStageScreenController centerStageScreenController)
    {
        this.config = config;
        this.settingsManager = settingsManager;
        this.songPreviewPlayer = songPreviewPlayer;
        this.centerStageScreenController = centerStageScreenController;
    }

    public void Initialize()
    {
        foreach (var previewAudioSource in songPreviewPlayer._audioSourceControllers.Select(x => x.audioSource))
        {
            previewAudioSource.DecorateAudioSource(config, settingsManager);
        }
    
        foreach (var uiAudioSource in BeatSaberUI.BasicUIAudioManager._audioSources)
        {
            uiAudioSource.DecorateAudioSource(config, settingsManager, speakerCreator =>
                speakerCreator.volumeMultiplier *= 3f);
        }
        
        Plugin.Log.Info($"Decorating {centerStageScreenController.name} {centerStageScreenController._countdownController.name} {centerStageScreenController._countdownController._audioSource.name}");
        centerStageScreenController._countdownController._audioSource.DecorateAudioSource(config, settingsManager);
    }
}