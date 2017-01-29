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
    public KeyboardConfig keyboard;
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
    public int m_activeNotes = 0;
    public float m_lastVolume = 0;
    public NOTE m_lastNote = NOTE.C;
    private NoteData m_noteData;
    bool m_notePlaying = false;
    bool m_useKeyboard = false;
	// Use this for initialization
	void Start ()
    {
        m_source = GetComponent<AudioSource>();

  //      if (Microphone.devices.Length > 0)
        {
            Debug.Log("has device");
            m_device = GameInfo.currentDevice;
            if (m_device == null) m_useKeyboard = true;
            else m_useKeyboard = (m_device == "Keyboard");
            
            Debug.Log("device name = " + m_device);
            m_hasDevice = true;
            m_sampleRate = AudioSettings.outputSampleRate;
            initialize();
        }

        m_noteData = new NoteData();
       
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
    void checkNoteEnd()
    {
        NoteData data = new NoteData();
        if (Input.GetKeyUp(keyboard.A))
        {
            data.midi = 69;
            data.note = NOTE.A;
            m_notePlaying = false;
            m_activeNotes--;
            if (m_activeNotes <= 0)
            {
                m_activeNotes = 0;
                if (OnNoteEnd != null) OnNoteEnd(data);
            }
        }
        if (Input.GetKeyUp(keyboard.As))
        {
            data.midi = 70;
            data.note = NOTE.As;
            m_notePlaying = false;
            m_activeNotes--;
            if (m_activeNotes <= 0)
            {
                m_activeNotes = 0;
                if (OnNoteEnd != null) OnNoteEnd(data);
            }
        }
        if (Input.GetKeyUp(keyboard.B))
        {
            data.midi = 71;
            data.note = NOTE.B;
            m_notePlaying = false;
            m_activeNotes--;
            if (m_activeNotes <= 0)
            {
                m_activeNotes = 0;
                if (OnNoteEnd != null) OnNoteEnd(data);
            }
        }
        if (Input.GetKeyUp(keyboard.C))
        {
            data.midi = 72;
            data.note = NOTE.C;
            m_notePlaying = false;
            m_activeNotes--;
            if (m_activeNotes <= 0)
            {
                m_activeNotes = 0;
                if (OnNoteEnd != null) OnNoteEnd(data);
            }
        }
        if (Input.GetKeyUp(keyboard.Cs))
        {
            data.midi = 73;
            data.note = NOTE.Cs;
            m_notePlaying = false;
            m_activeNotes--;
            if (m_activeNotes <= 0)
            {
                m_activeNotes = 0;
                if (OnNoteEnd != null) OnNoteEnd(data);
            }
        }

        if (Input.GetKeyUp(keyboard.D))
        {
            data.midi = 74;
            data.note = NOTE.D;
            m_notePlaying = false;
            m_activeNotes--;
            if (m_activeNotes <= 0)
            {
                m_activeNotes = 0;
                if (OnNoteEnd != null) OnNoteEnd(data);
            }
        }
        if (Input.GetKeyUp(keyboard.Ds))
        {
            data.midi = 75;
            data.note = NOTE.Ds;
            m_notePlaying = false;
            m_activeNotes--;
            if (m_activeNotes <= 0)
            {
                m_activeNotes = 0;
                if (OnNoteEnd != null) OnNoteEnd(data);
            }
        }
        if (Input.GetKeyUp(keyboard.E))
        {
            data.midi = 76;
            data.note = NOTE.E;
            m_notePlaying = false;
            m_activeNotes--;
            if (m_activeNotes <= 0)
            {
                m_activeNotes = 0;
                if (OnNoteEnd != null) OnNoteEnd(data);
            }
        }
        if (Input.GetKeyUp(keyboard.F))
        {
            data.midi = 77;
            data.note = NOTE.F;
            m_notePlaying = false;
            m_activeNotes--;
            if (m_activeNotes <= 0)
            {
                m_activeNotes = 0;
                if (OnNoteEnd != null) OnNoteEnd(data);
            }
        }
        if (Input.GetKeyUp(keyboard.Fs))
        {
            data.midi = 78;
            data.note = NOTE.Fs;
            m_notePlaying = false;
            m_activeNotes--;
            if (m_activeNotes <= 0)
            {
                m_activeNotes = 0;
                if (OnNoteEnd != null) OnNoteEnd(data);
            }
        }
        if (Input.GetKeyUp(keyboard.G))
        {
            data.midi = 79;
            data.note = NOTE.G;
            m_notePlaying = false;
            m_activeNotes--;
            if (m_activeNotes <= 0)
            {
                m_activeNotes = 0;
                if (OnNoteEnd != null) OnNoteEnd(data);
            }
        }
        if (Input.GetKeyUp(keyboard.Gs))
        {
            data.midi = 80;
            data.note = NOTE.Gs;
            m_notePlaying = false;
            m_activeNotes--;
            if (m_activeNotes <= 0)
            {
                m_activeNotes = 0;
                if (OnNoteEnd != null) OnNoteEnd(data);
            }
        }


    }
    bool keyboardInput(ref NoteData data)
    {
        if(Input.GetKeyDown(keyboard.A))
        {
            data.midi = 69;
            data.note = NOTE.A;
            m_activeNotes++;
            return true;
        }
        if (Input.GetKeyDown(keyboard.As))
        {
            data.midi = 70;
            data.note = NOTE.As;
            m_activeNotes++;
            return true;
        }
        if (Input.GetKeyDown(keyboard.B))
        {
            data.midi = 71;
            data.note = NOTE.B;
            m_activeNotes++;
            return true;
        }
        if (Input.GetKeyDown(keyboard.C))
        {
            data.midi = 72;
            data.note = NOTE.C;
            m_activeNotes++;
            return true;
        }
        if (Input.GetKeyDown(keyboard.Cs))
        {
            data.midi = 73;
            data.note = NOTE.Cs;
            m_activeNotes++;
            return true;
        }
        
        if (Input.GetKeyDown(keyboard.D))
        {
            data.midi = 74;
            data.note = NOTE.D;
            m_activeNotes++;
            return true;
        }
        if (Input.GetKeyDown(keyboard.Ds))
        {
            data.midi = 75;
            data.note = NOTE.Ds;
            m_activeNotes++;
            return true;
        }
        if (Input.GetKeyDown(keyboard.E))
        {
            data.midi = 76;
            data.note = NOTE.E;
            m_activeNotes++;
            return true;
        }
        if (Input.GetKeyDown(keyboard.F))
        {
            data.midi = 77;
            data.note = NOTE.F;
            m_activeNotes++;
            return true;
        }
        if (Input.GetKeyDown(keyboard.Fs))
        {
            data.midi = 78;
            data.note = NOTE.Fs;
            m_activeNotes++;
            return true;
        }
        if (Input.GetKeyDown(keyboard.G))
        {
            data.midi = 79;
            data.note = NOTE.G;
            m_activeNotes++;
            return true;
        }
        if (Input.GetKeyDown(keyboard.Gs))
        {
            data.midi = 80;
            data.note = NOTE.Gs;
            m_activeNotes++;
            return true;
        }

        return false;
    }

    void Update()
    {
        if (m_useKeyboard)
        {
            if (keyboardInput(ref m_noteData))
            {
                m_notePlaying = true;
                m_noteData.frequency = mtof(m_noteData.midi);
                if (OnNoteStart != null) OnNoteStart(m_noteData);
            }
            checkNoteEnd();
        }
    }
	
	// Update is called once per frame
	void LateUpdate()
    {
        if (m_useKeyboard) return;  
        if (m_notePlaying) return;
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
            if (midi < 30) return;
            if (OnNoteStart != null) OnNoteStart(noteData);
        }
        else if(volume > averageVolumeThreshold && m_lastVolume > averageVolumeThreshold)
        {
            if (midi < 30) return;
            if (m_lastNote != note)
            {
                if(OnNoteChange != null) OnNoteChange(noteData);
            }
        }
        
        m_lastVolume = volume;
        m_lastNote = note;
	}
}
