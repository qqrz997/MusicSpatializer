using UnityEngine;
using UnityEngine.Audio;

namespace MusicSpatializer.Components;

internal class SpeakerCreator : MonoBehaviour
{
    // Initialized on creation
    private PluginConfig config = null!;
    private AudioSplitter splitter = null!;
    private AudioMixerGroup? mixerGroup;

    public void Initialize(PluginConfig config, AudioSplitter splitter, AudioMixerGroup? mixerGroup)
    {
        this.config = config;
        this.splitter = splitter;
        this.mixerGroup = mixerGroup;
    }
    
    public AudioClip? dclip;
    public GameObject? speakerLeft;
    public GameObject? speakerRight;
    public GameObject? speakerResonance;
    public GameObject? speakerBass;
    public float frontDistance = 20;
    public float sideDistance = 15f;
    public float volumeMultiplier = 1;
    private GameObject? rotationMarker;
    private int rotationMarkerTries = 10;

    private void Start()
    {
        Create();
    }

    private void Update()
    {
        //sideDistance += 0.01f;
        //Create();
        if (config.enable360)
        {
            //rotate audio based on the chevron in 360 maps
            if (rotationMarker != null)
            {
                //Console.WriteLine("found Chevron");

                transform.rotation = rotationMarker.transform.rotation;
            }
            else
            {
                //Console.WriteLine("not found Chevron");
                if (rotationMarkerTries > 0)
                {
                    //rotationMarker = GameObject.Find("SpawnRotationChevron");
                    rotationMarker = GameObject.Find("FlyingGameHUD");
                    if (rotationMarker != null)
                    {
                        sideDistance = 7f;
                        PositionSpeakers();
                    }
                    rotationMarkerTries--;
                }
            }
        }
    }

    private GameObject NewSpeaker(int channel)
    {
        var speaker = config.debugSpheres ? GameObject.CreatePrimitive(PrimitiveType.Sphere) : new();
        speaker.transform.parent = transform;
        speaker.name = "MusicSpatializerSpeaker";

        var reader = speaker.AddComponent<AudioReader>();
        reader.channel = channel;
        reader.Initialize(config, splitter);

        var source = speaker.AddComponent<AudioSource>();
        if (mixerGroup != null)
        {
            source.outputAudioMixerGroup = mixerGroup;
        }

        source.spatialize = config.enableSpatialize;
        source.spatializePostEffects = config.enableSpatialize;
        source.bypassEffects = false;
        source.bypassListenerEffects = true;
        source.bypassReverbZones = true;
        source.dopplerLevel = 0;
        source.clip = dclip;
        source.rolloffMode = AudioRolloffMode.Linear;
        source.minDistance = 1000000;
        source.maxDistance = 1000000;
        source.spatialBlend = config.enableSpatialize ? 1 : 0;
        source.volume = 1;
        reader.volume = (config.enableSpatialize ? 0.275f : 0.4f) * volumeMultiplier;
        source.priority = 0;
        source.ignoreListenerPause = true;
        source.Play();
        if (channel == -1)
        {
            source.spatialize = false;
            reader.volume = 0.2f * volumeMultiplier;
            source.reverbZoneMix = 1.0f;
            source.spatialBlend = 0;
            source.bypassReverbZones = false;

            //AudioLowPassFilter lowpass = speaker.AddComponent<AudioLowPassFilter>();
            //lowpass.cutoffFrequency = 350;

            speaker.AddComponent<Silencer>();

            AudioReverbZone reverb = speaker.AddComponent<AudioReverbZone>();
            reverb.reverbPreset = AudioReverbPreset.Hangar;
        }
        else if (channel == 21)
        {
            AudioLowPassFilter lowpass = speaker.AddComponent<AudioLowPassFilter>();
            lowpass.cutoffFrequency = 300;
            //source.spatialize = false;
            reader.volume = 0.5f * volumeMultiplier;
        }
        return speaker;
    }

    private void PositionSpeakers()
    {
        if (speakerLeft != null)
        {
            speakerLeft.transform.localPosition = new Vector3(-sideDistance, 1.5f, frontDistance);
        }
        if (speakerRight != null)
        {
            speakerRight.transform.localPosition = new Vector3(sideDistance, 1.5f, frontDistance);
        }
        if (speakerResonance != null)
        {
            speakerResonance.transform.localPosition = new Vector3(0, 3, 0);
        }
        if (speakerBass != null)
        {
            speakerBass.transform.localPosition = new Vector3(0, -3f, 10f);
        }
    }

    private void Create()
    {
        if (speakerLeft)
        {
            Destroy(speakerLeft);
        }
        if (speakerRight)
        {
            Destroy(speakerRight);
        }
        speakerLeft = NewSpeaker(0);
        speakerRight = NewSpeaker(1);
        if (config.enableResonance)
        {
            speakerResonance = NewSpeaker(-1);
        }
        if (config.enableBassBoost)
        {
            speakerBass = NewSpeaker(21);
        }
        PositionSpeakers();
    }
}