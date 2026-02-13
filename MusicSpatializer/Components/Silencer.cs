using UnityEngine;

namespace MusicSpatializer.Components;

internal class Silencer : MonoBehaviour
{
    private void OnAudioFilterRead(float[] data, int channels)
    {
        int dataLen = data.Length / channels;

        int n = 0;
        while (n < dataLen)
        {

            int i = 0;
            while (i < channels)
            {
                data[n * channels + i] = 0;
                i++;
            }
            n++;
        }
    }
}