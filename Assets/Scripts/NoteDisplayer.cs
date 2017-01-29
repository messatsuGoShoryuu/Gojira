using UnityEngine;
using System.Collections;

public class NoteDisplayer : MonoBehaviour
{
    TextMesh m_textMesh;
	// Use this for initialization
	void Start ()
    {
        m_textMesh = GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnEnable()
    {
        AudioInput.OnNoteChange += AudioInput_OnNoteChange;
        AudioInput.OnNoteEnd += AudioInput_OnNoteEnd;
        AudioInput.OnNoteStart += AudioInput_OnNoteStart;
    }

    private void AudioInput_OnNoteStart(NoteData note)
    {
        m_textMesh.text = "Note = " + note.note + " Frequency = " + note.frequency;
    }

    private void AudioInput_OnNoteEnd(NoteData note)
    {
        m_textMesh.text = "No Input";
    }

    private void AudioInput_OnNoteChange(NoteData note)
    {
        m_textMesh.text = "Note = " + note.note + " Frequency = " + note.frequency;
    }

    void OnDisable()
    {
        AudioInput.OnNoteChange -= AudioInput_OnNoteChange;
        AudioInput.OnNoteEnd -= AudioInput_OnNoteEnd;
        AudioInput.OnNoteStart -= AudioInput_OnNoteStart;
    }
}
