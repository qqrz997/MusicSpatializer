using MusicSpatializer.Extensions;
using UnityEngine;
using Zenject;

namespace MusicSpatializer.Services;

internal class SpatializerMultiplayerInitializer : IInitializable
{
    private readonly PluginConfig config;
    private readonly SettingsManager settingsManager;
    private readonly MultiplayerIntroAnimationController introAnimationController;
    private readonly MultiplayerOutroAnimationController outroAnimationController;

    public SpatializerMultiplayerInitializer(PluginConfig config,
        SettingsManager settingsManager,
        MultiplayerIntroAnimationController introAnimationController,
        MultiplayerOutroAnimationController outroAnimationController)
    {
        this.config = config;
        this.settingsManager = settingsManager;
        this.introAnimationController = introAnimationController;
        this.outroAnimationController = outroAnimationController;
    }

    public void Initialize()
    {
        var countdownSound = introAnimationController.GetComponentInChildren<AudioSource>(true);
        if (countdownSound != null)
        {
            countdownSound.DecorateAudioSource(config, settingsManager);
        }
        else
        {
            Plugin.Log.Critical($"Unable to get {nameof(AudioSource)} from {nameof(introAnimationController)}");
        }
        
        var outroAudio = outroAnimationController.transform.Find("OutroAudio");
        if (outroAudio != null && outroAudio.TryGetComponent<AudioSource>(out var outroAudioSource))
        {
            outroAudioSource.DecorateAudioSource(config, settingsManager);
        }
        else
        {
            Plugin.Log.Critical($"Unable to get {nameof(AudioSource)} from {nameof(outroAnimationController)}");
        }
     
        // todo - might need to get the AudioSource from:
        //      - MultiplayerLocalActivePlayerController(Clone)/MultiplayerLocalActiveOutroAnimator
        //      - MultiplayerLocalInactivePlayerOutroAnimator
    }
}