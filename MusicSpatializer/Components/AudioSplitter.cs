using UnityEngine;

namespace MusicSpatializer.Components;

internal class AudioSplitter : MonoBehaviour
{

    public float[][] channelData = [];
    public bool[] beenUsed = [];
    public int iteration = 0;
    public bool ready = false;

    private AudioSource? source;
    //float lastTime = 0;

    private void Update()
    {
        if (!source)
        {
            source = gameObject.GetComponent<AudioSource>();
        }

        source.spatialBlend = 0;
        source.reverbZoneMix = 0;
        source.dopplerLevel = 0;
        source.bypassEffects = false;
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        int dataLen = data.Length / channels;
        if (channelData.Length != channels)
        {
            channelData = new float[channels][];
            beenUsed = new bool[channels];
        }

        for (int channelIdx = 0; channelIdx < channels; channelIdx++)
        {
            var channel = channelData[channelIdx];
            // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
            if (channel?.Length != dataLen) 
                channelData[channelIdx] = new float[dataLen];
            beenUsed[channelIdx] = false;
        }
        iteration++;
        
        for (int n = 0; n < dataLen; n++)
        {
            for (int i = 0; i < channels; i++)
            {
                channelData[i][n] = data[n * channels + i];
                data[n * channels + i] = 0;
            }
        }
        ready = true;
    }
}