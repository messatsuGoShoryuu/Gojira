using UnityEngine;
using System.Collections;

public class GuitarPlayer : MonoBehaviour
{
    private Synth m_synth;
    private AudioEnvelope m_envelope;

    public float decayRate;
    bool m_decay = false;

	// Use this for initialization
	void Start ()
    {
        m_synth = GetComponent<Synth>();
        m_envelope = GetComponent<AudioEnvelope>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (DamageZone.blocked) m_synth.gain = 0.0f;
        else if (m_decay) m_synth.gain = Mathf.Lerp(m_synth.gain, 0.0f, decayRate * Time.deltaTime);
        else m_synth.gain = m_envelope.gain();
        
	}

    void OnEnable()
    {
        AudioInput.OnNoteChange += AudioInput_OnNoteChange;
        AudioInput.OnNoteEnd += AudioInput_OnNoteEnd;
        AudioInput.OnNoteStart += AudioInput_OnNoteStart;
    }

    private void AudioInput_OnNoteStart(NoteData note)
    {
        m_synth.frequency = note.frequency;
        m_envelope.play();
        m_decay = false;
    }

    private void AudioInput_OnNoteEnd(NoteData note)
    {
        m_synth.gain = 0.0f;
        m_decay = true;
    }

    private void AudioInput_OnNoteChange(NoteData note)
    {
        m_synth.frequency = note.frequency;
        m_envelope.play();
        m_decay = false;
    }

    void OnDisable()
    {
        AudioInput.OnNoteChange -= AudioInput_OnNoteChange;
        AudioInput.OnNoteEnd -= AudioInput_OnNoteEnd;
        AudioInput.OnNoteStart -= AudioInput_OnNoteStart;
    }

    
}
