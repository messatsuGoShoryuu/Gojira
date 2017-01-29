using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shred : MonoBehaviour
{
    public delegate void EndShred();
    public static event EndShred OnEndShred;
    HashSet<int> m_noteSet;

    SpriteRenderer m_spriteRenderer;
    public Sprite[] sprites;
    public AudioSource shredCountdownAudio;
    int m_spriteCounter;

    public Ship ship;
    public static float shipHealth = 60;

    int m_noteCount = 0;
    int m_noteDiversity = 0;

    AudioSource m_source;
    public AudioSource gameplayMusic;
    public ShredBar shredBar;
    static bool m_isShredding = false;

    static float m_damage;

    Gojira m_gojira;
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
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_source = GetComponent<AudioSource>();
        m_noteSet = new HashSet<int>();
        m_gojira = FindObjectOfType<Gojira>();
        shredBar.gameObject.SetActive(false);
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
        shredBar.gameObject.SetActive(true);
        shredBar.setHealth(0);
        m_gojira.laneSelector.SetActive(false);
        m_spriteCounter = 0;
        m_damage = 0.0f;
        gameplayMusic.Stop();
        ship.gameObject.SetActive(true);
        ship.arrive();

        m_source.Play();
        DamageZone.block();
        yield return new WaitForSeconds(8.0f);
        m_spriteRenderer.enabled = true;
        m_spriteRenderer.sprite = sprites[m_spriteCounter];
        m_spriteCounter++;

        yield return new WaitForSeconds(1.0f);
        m_spriteRenderer.sprite = sprites[m_spriteCounter];
        m_spriteCounter++;
        shredCountdownAudio.Play();

        yield return new WaitForSeconds(1.0f);
        m_spriteRenderer.sprite = sprites[m_spriteCounter];
        m_spriteCounter++;
        shredCountdownAudio.Play();

        yield return new WaitForSeconds(1.0f);
        m_spriteRenderer.sprite = sprites[m_spriteCounter];
        m_spriteCounter++;
        shredCountdownAudio.Play();

        yield return new WaitForSeconds(1.0f);
        m_spriteRenderer.sprite = sprites[m_spriteCounter];
        m_spriteCounter++;
        shredCountdownAudio.Play();
      

        yield return new WaitForSeconds(1.0f);
        m_spriteRenderer.enabled = false;

        DamageZone.unblock();
        m_gojira.shred();

        m_isShredding = true;
        StartCoroutine(outro());
    }

    void Update()
    {
        if(m_isShredding)
            calculateDamage();
    }

    void calculateDamage()
    {
        m_damage = (m_noteCount) / shipHealth;
        if (m_damage >= 1.0f) m_damage = 1.0f;
        shredBar.setHealth(m_damage);
        Debug.Log("note count = " + m_noteCount + " damage = " + m_damage);
    }

    IEnumerator outro()
    {

        yield return new WaitForSeconds(12.0f);
        m_isShredding = false;

        if (m_damage >= 1.0f) ship.explode();
        else ship.fire();
        
        float health =  FindObjectOfType<DamageZone>().health / 10.0f;
        bool wellDone = health > (1.0f - m_damage);
        Debug.Log("health = " + health + " damage = " + m_damage);
        DamageZone.block();
        if (wellDone)
        {
            yield return new WaitForSeconds(2.0f);
            shredBar.gameObject.SetActive(false);
            m_spriteRenderer.enabled = true;
            m_spriteRenderer.sprite = sprites[m_spriteCounter];
        }
        yield return new WaitForSeconds(1.0f);
        m_spriteRenderer.enabled = false;
        Debug.Log("damage = " + m_damage);
        DamageZone.unblock();
        endShred();
        m_gojira.laneSelector.SetActive(true);
        ship.gameObject.SetActive(false);
        
    }

    private void endShred()
    {
        m_source.Stop();
        gameplayMusic.Play();
        if (OnEndShred != null) OnEndShred();
        m_gojira.play();
    }


}
