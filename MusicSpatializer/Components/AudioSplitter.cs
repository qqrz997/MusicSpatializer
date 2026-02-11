using UnityEngine;

namespace MusicSpatializer.Components;

public class AudioSplitter : MonoBehaviour
{

    public float[][] channelData = [];
    public bool[] beenUsed = [];
    public int iteration = 0;
    public bool ready = false;
    AudioSource? source;
    //float lastTime = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (source == null)
        {
            source = gameObject.GetComponent<AudioSource>();
            source.spatialBlend = 0;
            source.reverbZoneMix = 0;
            source.dopplerLevel = 0;
            source.bypassEffects = false;
        }
        else
        {
            source.spatialBlend = 0;
            source.reverbZoneMix = 0;
            source.dopplerLevel = 0;
            source.bypassEffects = false;
            /*if (source.time == 0)
            {
                source.time = lastTime;
                source.Play();
            }
            lastTime = source.time;*/
        }
    }

    void OnAudioFilterRead(float[] data, int channels)
    {
        int dataLen = data.Length / channels;
        if (channelData?.Length != channels)
        {
            channelData = new float[channels][];
            beenUsed = new bool[channels];
        }
        int c = 0;
        while (c < channels)
        {
            if (channelData[c]?.Length != dataLen)
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