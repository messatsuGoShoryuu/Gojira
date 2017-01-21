using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shred : MonoBehaviour
{
    public delegate void EndShred();
    public static event EndShred OnEndShred;

    TextMesh m_noteText;
    public TextMesh announceText;

    HashSet<int> m_noteSet;

    public Ship ship;
    public static int shipHealth = 60;

    int m_noteCount = 0;
    int m_noteDiversity = 0;

    AudioSource m_source;
    public AudioSource gameplayMusic;
    static bool m_isShredding = false;

    static float m_damage;
    public static float damage
    {
        get
        {
            return m_damage;
        }
    }

    public static bool isShredding
    {
        get
        {
            return m_isShredding;
        }
    }

	// Use this for initialization
	void Start ()
    {
        m_source = GetComponent<AudioSource>();
        m_noteSet = new HashSet<int>();
        m_noteText = GetComponent<TextMesh>();
	}

    void OnEnable()
    {
        SpawnScheduler.OnStartShred += SpawnScheduler_OnStartShred;
        AudioInput.OnNoteStart += AudioInput_OnNoteStart;
        AudioInput.OnNoteChange += AudioInput_OnNoteStart;
    }

    private void AudioInput_OnNoteStart(NoteData note)
    {
        if (m_isShredding)
        {
            m_noteSet.Add(note.midi);
            m_noteCount++;
            m_noteText.text = m_noteCount.ToString() + " NOTES, " + m_noteSet.Count + " DIFFERENT NOTES";
        }
    }

    void OnDisable()
    {
        SpawnScheduler.OnStartShred -= SpawnScheduler_OnStartShred;
        AudioInput.OnNoteStart -= AudioInput_OnNoteStart;
        AudioInput.OnNoteChange -= AudioInput_OnNoteStart;
    }

    private void SpawnScheduler_OnStartShred()
    {
        m_noteCount = 0;
        m_noteDiversity = 0;
        m_source.Play();
        m_noteSet.Clear();
        StartCoroutine(intro());
        
    }

    IEnumerator intro()
    {
        gameplayMusic.Stop();
        ship.gameObject.SetActive(true);
        ship.arrive();
        ship.setHealth(shipHealth);
        m_source.Play();
        MeshRenderer meshRenderer = announceText.GetComponent<MeshRenderer>();
        DamageZone.block();
        yield return new WaitForSeconds(8.0f);
        meshRenderer.enabled = true;
        announceText.text = "LET'S SHREDDING!";
        yield return new WaitForSeconds(1.0f);
        announceText.text = "3";
        yield return new WaitForSeconds(1.0f);
        announceText.text = "2";
        yield return new WaitForSeconds(1.0f);
        announceText.text = "1";
        yield return new WaitForSeconds(1.0f);
        announceText.text = "GO!";
        yield return new WaitForSeconds(1.0f);
        meshRenderer.enabled = false;

        DamageZone.unblock();

        m_isShredding = true;
        StartCoroutine(outro());
    }

    IEnumerator outro()
    {

        MeshRenderer meshRenderer = announceText.GetComponent<MeshRenderer>();
        yield return new WaitForSeconds(12.0f);
        m_isShredding = false;
        m_damage = (80.0f + m_noteSet.Count) / 100.0f;
        DamageZone.block();
        yield return new WaitForSeconds(2.0f);
        meshRenderer.enabled = true;
        announceText.text = "WELL DONE!";
        yield return new WaitForSeconds(1.0f);
        meshRenderer.enabled = false;
        Debug.Log("damage = " + m_damage);
        DamageZone.unblock();
        endShred();
        ship.gameObject.SetActive(false);
        
    }

    private void endShred()
    {
        m_source.Stop();
        gameplayMusic.Play();
        if (OnEndShred != null) OnEndShred();
    }


}
