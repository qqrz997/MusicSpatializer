using UnityEngine;

namespace MusicSpatializer.Components;

public class Silencer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnAudioFilterRead(float[] data, int channels)
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