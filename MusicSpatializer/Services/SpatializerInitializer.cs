using System;
using System.Linq;
using MusicSpatializer.Components;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;
using Object = UnityEngine.Object;

namespace MusicSpatializer.Services;

internal class SpatializerInitializer : IInitializable
{
    private readonly PluginConfig config;
    private readonly SettingsManager settingsManager;
    private readonly SongController? songController;
    private readonly BasicUIAudioManager? basicUIAudioManager;
    private readonly SongPreviewPlayer? songPreviewPlayer;
    
    public SpatializerInitializer(
        PluginConfig config,
        SettingsManager settingsManager,
        [InjectOptional] SongController? songController,
        [InjectOptional] BasicUIAudioManager? basicUIAudioManager,
        [InjectOptional] SongPreviewPlayer? songPreviewPlayer)
    {
        this.config = config;
        this.settingsManager = settingsManager;
        this.songController = songController;
        this.basicUIAudioManager = basicUIAudioManager;
        this.songPreviewPlayer = songPreviewPlayer;
    }
    
    public void Initialize()
    {
        if (!config.enabled)
        {
            return;
        }

        if (songController != null && songController.TryGetComponent<AudioSource>(out var songControllerAudioSource))
        {
            DecorateAudioSource(songControllerAudioSource);
            // songControllerAudioSource.gameObject.AddComponent<MainSongAudioSourceRestarterBugFix>();
        }

        if (basicUIAudioManager != null && basicUIAudioManager.TryGetComponent<AudioSource>(out var uiAudioSource))
        {
            DecorateAudioSource(uiAudioSource, speakerCreator => speakerCreator.volumeMultiplier *= 3f);
        }

        if (songPreviewPlayer != null)
        {
            foreach (var previewAudioSource in songPreviewPlayer._audioSourceControllers.Select(x => x.audioSource))
            {
                DecorateAudioSource(previewAudioSource);
            }
        }
        
        foreach (var go in Object.FindObjectsOfType<GameObject>())
        {
            if (!go.activeInHierarchy || go.name
                    is not ("MultiplayerLocalInactivePlayerSongSyncController"
                    or "MultiplayerLocalInactivePlayerOutroAnimator"
                    or "MultiplayerLocalActiveOutroAnimator"
                    or "CountdownSound"
                    or "OutroAudio"))
            {
                continue;
            }

            if (go.TryGetComponent<AudioSource>(out var audioSource))
            {
                DecorateAudioSource(audioSource);
            }
        }
    }

    private void DecorateAudioSource(AudioSource audioSource, Action<SpeakerCreator>? decorator = null)
    {
        // prevent from it being attached multiple times
        if (!audioSource.gameObject.activeInHierarchy || audioSource.gameObject.GetComponent<AudioSplitter>() != null)
        {
            return;
        }
        
        var baseObject = new GameObject("MusicSpatializerBase");
        baseObject.transform.SetParent(audioSource.transform, false);


        var volumeMultiplier = settingsManager.settings.audio.volume * config.musicVolumeMultiplier;
        if (config.enableBassBoost)
        {
            volumeMultiplier *= 0.75f;
        }

        var audioSplitter = audioSource.gameObject.AddComponent<AudioSplitter>();
        var speakerCreator = baseObject.AddComponent<SpeakerCreator>();

        AudioMixerGroup? mixerGroup = null;
        if(!config.enableSpatialize && audioSource.outputAudioMixerGroup != null)
        {
            // find master group so that 2d audio works
            mixerGroup = audioSource.outputAudioMixerGroup.audioMixer
                .FindMatchingGroups("")
                .First(x => x.name == "Master");
        }
        speakerCreator.Initialize(config, audioSplitter, mixerGroup);
        speakerCreator.volumeMultiplier = volumeMultiplier;
        
        decorator?.Invoke(speakerCreator);
    }
}