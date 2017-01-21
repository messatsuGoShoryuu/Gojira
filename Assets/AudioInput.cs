using UnityEngine;
using System.Collections;

public enum NOTE
{
    C,
    Cs,
    D,
    Ds,
    E,
    F,
    Fs,
    G,
    Gs,
    A,
    As,
    B
}

public struct NoteData
{
    public NOTE note;
    public float frequency;
    public int midi;
}
public class AudioInput : MonoBehaviour
{
    public delegate void NoteStart(NoteData note);
    public static event NoteStart OnNoteStart;

    public delegate void NoteEnd(NoteData note);
    public static event NoteEnd OnNoteEnd;

    public delegate void NoteChange(NoteData note);
    public static event NoteChange OnNoteChange;

    public int fftSampleCount;
    
    private AudioSource m_source;
    private float m_maxFrequency;
    private float m_minFrequency;
    private float[] m_buffer;

    private float[] m_volumeBuffer;
    private string m_device;
    private int m_sampleRate;
    bool m_hasDevice = false;
    public float volumeThreshold;
    public float averageVolumeThreshold;

    float[] m_midiFreq;

    public float m_lastVolume = 0;
    public NOTE m_lastNote = NOTE.C;
	// Use this for initialization
	void Start ()
    {
        m_source = GetComponent<AudioSource>();
        if (Microphone.devices.Length > 0)
        {
            Debug.Log("has device");
            m_device = Microphone.devices[0];
            Debug.Log("device name = " + m_device);
            m_hasDevice = true;
            m_sampleRate = AudioSettings.outputSampleRate;
            initialize();
        }

       
        m_midiFreq = new float[127];
        float a = 440; 
        for (int i = 0; i < 127; ++i)
        {
            float f = i - 69;
            m_midiFreq[i] = Mathf.Pow(2.0f, (f / 12.0f)) * a;
        }
    }

    public float mtof(int midi)
    {
        if (midi < 0 || midi >= 127) return 0.0f;
       
        return m_midiFreq[midi];
    }

    int ftom(float frequency)
    {
        float result =  17.3123404907f * Mathf.Log(frequency / 27.5f) + 21;
        return (int)result + 1;
    }

    void initialize()
    {
        if(m_hasDevice)
        {
            m_maxFrequency = AudioSettings.outputSampleRate / 2;
            m_buffer = new float[fftSampleCount];
            m_volumeBuffer = new float[256];
            m_source.clip = Microphone.Start(m_device, true, 999, (m_sampleRate));
            m_source.loop = true; // Set the AudioClip to loop
            while (!(Microphone.GetPosition(m_device) > 0)) { } // Wait until the recording has started
            m_source.Play(); // Play the audio source!
        }
    }

    IEnumerator lateStart()
    {
        yield return new WaitForSeconds(0.2f);
        m_source.Play();
    }

    public NOTE midiToNote(int midi)
    {
        return (NOTE)(midi % 12);
    }

    float getVolume()
    { 
        float a = 0;
        m_source.GetOutputData(m_volumeBuffer, 0);
        foreach (float s in m_volumeBuffer)
        {
            a += Mathf.Abs(s);
        }
        return a / 256;
    }
    

    float fundamental()
    {
        if (!m_hasDevice) return 0.0f;
        
        m_source.GetSpectrumData(m_buffer, 0, FFTWindow.BlackmanHarris);
        float loudest = 0.0f;
        int index = 0;
        for(int i = 1; i<m_buffer.Length;i++)
        {
            if(m_buffer[i] > loudest && m_buffer[i] > volumeThreshold)
            {
                loudest = m_buffer[i];
                index = i;
            }
        }
        

        float indexF = (float)index;
        if (index > 0 && index < fftSampleCount - 1)
        { // interpolate index using neighbours
            float dL = m_buffer[index - 1] / m_buffer[index];
            float dR = m_buffer[index + 1] / m_buffer[index];
            indexF += 0.5f * (dR * dR - dL * dL);
        }


        float result = indexF * (m_sampleRate / 2) / m_buffer.Length;
        return result;
    }
	
	// Update is called once per frame
	void LateUpdate()
    {
        float fundamentalFreq = fundamental();
        int midi = ftom(fundamentalFreq);
        NOTE note = midiToNote(midi);
        float volume = getVolume();
        if ((int)note < 0) volume = 0;
        NoteData noteData = new NoteData();
        noteData.note = note;
        noteData.midi = midi;
        
        noteData.frequency = mtof(midi);
        if(m_lastVolume >= averageVolumeThreshold && volume < averageVolumeThreshold)
        {
            if (OnNoteEnd != null) OnNoteEnd(noteData);
        }
        else if (m_lastVolume <= averageVolumeThreshold && volume > averageVolumeThreshold)
        {
            if (OnNoteStart != null) OnNoteStart(noteData);
        }
        else if(volume > averageVolumeThreshold && m_lastVolume > averageVolumeThreshold)
        {
            if(m_lastNote != note)
            {
                if(OnNoteChange != null) OnNoteChange(noteData);
            }
        }
        
        m_lastVolume = volume;
        m_lastNote = note;
	}
}
