using UnityEngine;
using System.Collections;

public class Synth : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	
	}

    float m_phase;
    float m_displacement;
    int m_samplingRate;

    public float frequency;
    public float gain;


    // Update is called once per frame
    void Update ()
    {
        m_samplingRate = AudioSettings.outputSampleRate;
	}

    //taken from http://www.develop-online.net/tools-and-tech/procedural-audio-with-unity/0117433,
    //changed variable / method names to fit personal convention.

    void OnAudioFilterRead(float[] data, int channels)
    {
        // update increment in case frequency has changed
        m_displacement = frequency * 2.0f * Mathf.PI / m_samplingRate;
        for (int i = 0; i < data.Length; i = i + channels)
        {
            m_phase = m_phase + m_displacement;
            // this is where we copy audio data to make them “available” to Unity
            data[i] = (float)(gain * Mathf.Sign(Mathf.Sin(m_phase)));
            // if we have stereo, we copy the mono data to each channel
            if (channels == 2) data[i + 1] = data[i];
            if (m_phase > 2 * Mathf.PI) m_phase = 0;
        }
    }
} 


