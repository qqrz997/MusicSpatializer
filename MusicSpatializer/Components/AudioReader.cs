using UnityEngine;

namespace MusicSpatializer.Components;

internal class AudioReader : MonoBehaviour
{
    // Initialized on creation
    private AudioSplitter splitter = null!;
    private PluginConfig config = null!;

    public void Initialize(PluginConfig config, AudioSplitter splitter)
    {
        this.config = config;
        this.splitter = splitter;
        allChannels = channel is -1 or 21;
    }
    
    public int channel = 0;
    private bool allChannels = false;
    public float volume = 1.0f;
    private int iteration = 0;
    private AudioSource? source;

    // Update is called once per frame
    private void Update()
    {
        if (!source)
        {
            source = gameObject.GetComponent<AudioSource>();
            return;
        }

        if (source!.isPlaying)
        {
            return;
        }

        if (config.debugSpheres)
        {
            Plugin.Log.Info("Speaker restarted");
        }
        source.Play();
    }


    private void OnAudioFilterRead(float[] data, int channels)
    {
        //Plugin.Log("reading audio {0} channels", channels);

        if (splitter.ready == false)
        {
            return;
        }

        //only run if there is new data
        if (splitter.iteration - iteration <= 0)
        {
            return;
        }
        iteration = splitter.iteration;

        int dataLen = data.Length / channels;
        int n = 0;

        if (allChannels)
        {
            while (n < dataLen)
            {

                int i = 0;
                while (i < channels)
                {
                    data[n * channels + i] = splitter.channelData[i][n] * volume;
                    i++;
                }
                n++;
            }
        }
        else
        {
            float[] slitData = splitter.channelData[channel];
            while (n < dataLen)
            {
                if (config.enableSpatialize)
                {
                    int i = 0;
                    while (i < channels)
                    {
                        data[n * channels + i] = slitData[n] * volume;
                        i++;
                    }
                } 
                else
                {
                    data[n * channels + channel] = slitData[n] * volume;
                }
                    
                n++;
            }
        }
    }
}