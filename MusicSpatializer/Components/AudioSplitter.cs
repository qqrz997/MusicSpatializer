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
        int c = 0;
        while (c < channels)
        {
            if (channelData[c].Length != dataLen)
            {
                channelData[c] = new float[dataLen];
            }
            beenUsed[c] = false;
            c++;
        }
        iteration++;

        int n = 0;
        while (n < dataLen)
        {

            int i = 0;
            while (i < channels)
            {
                channelData[i][n] = data[n * channels + i];
                data[n * channels + i] = 0;
                i++;
            }
            n++;
        }
        ready = true;
    }
}