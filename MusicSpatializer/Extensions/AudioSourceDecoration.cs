using System;
using System.Linq;
using MusicSpatializer.Components;
using UnityEngine;
using UnityEngine.Audio;

namespace MusicSpatializer.Extensions;

internal static class AudioSourceDecoration
{
    public static void DecorateAudioSource(this AudioSource audioSource,
        PluginConfig config,
        SettingsManager settingsManager,
        Action<SpeakerCreator>? decorator = null)
    {
        // prevent from it being attached multiple times
        if (audioSource.gameObject.TryGetComponent<AudioSplitter>(out var audioSplitter))
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

        audioSplitter = audioSource.gameObject.AddComponent<AudioSplitter>();
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
        
        Plugin.Log.Info($"Created Audio Spatializer on {audioSource.transform.parent.name}/{audioSource.name}");
    }
}