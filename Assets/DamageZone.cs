using UnityEngine;
using System.Collections;


public class DamageZone : MonoBehaviour
{
    static bool m_blocked = false;
    public float penaltyLength;
     int m_health = 100;
    public int m_failCount = 0;

    public delegate void ChangeHealth(int health);
    public static event ChangeHealth OnChangeHealth;

    public delegate void JamStart();
    public static event JamStart OnJamStart;

    public delegate void JamEnd();
    public static event JamEnd OnJamEnd;

    public int shredTime;
    bool m_lastSuccess = false;
    int m_consecutiveSuccess = 0;
    public static bool blocked
    {
        get
        {
            return m_blocked;
        }
    }

    public static void block()
    {
        m_blocked = true;
    }

    public static void unblock()
    {
        m_blocked = false;
    }
	// Use this for initialization
	void Start ()
    {
        m_availableNotes = new Enemy[12];
        m_noteCount = 0;
	}

    Enemy[] m_availableNotes;
    int m_noteCount;
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void addNote(Enemy note)
    {
        m_availableNotes[m_noteCount] = note;
        m_noteCount++;
    }

    bool removeNote(NOTE note)
    {
        for(int i = 0; i<m_noteCount;i++)
        {
            if(m_availableNotes[i].note == note)
            {
                m_noteCount--;
                Enemy temp = m_availableNotes[m_noteCount];
                GameObject.Destroy(m_availableNotes[i].gameObject);
                NoteSpawner.enemyCount--;
                if (NoteSpawner.enemyCount < 0) NoteSpawner.enemyCount = 0;
                m_availableNotes[i] = temp;
                m_availableNotes[m_noteCount] = null;
                return true;
            }
        }
        return false;
    }

    void removeEnemy(Enemy enemy)
    {
        for (int i = 0; i < m_noteCount; i++)
        {
            if (m_availableNotes[i] == enemy)
            {
                m_noteCount--;
                GameObject.Destroy(enemy.gameObject);
                NoteSpawner.enemyCount--;
                Enemy temp = m_availableNotes[m_noteCount];
                m_availableNotes[i] = temp;
                m_availableNotes[m_noteCount] = null;
            }
        }
    }

    void OnEnable()
    {
            AudioInput.OnNoteStart += AudioInput_OnNoteStart;
            AudioInput.OnNoteChange += AudioInput_OnNoteStart;
    }

    private void AudioInput_OnNoteStart(NoteData note)
    {
        if (m_blocked || Shred.isShredding) return;
        
        bool success = removeNote(note.note);
        if (!success)
        {
            m_failCount++;
            m_consecutiveSuccess = 0;
        }
        else if (success && m_lastSuccess)
        {
            m_consecutiveSuccess++;
        }
        if(m_consecutiveSuccess > 3 && m_failCount > 0)
        {
            m_failCount--;
        }
       
        if (m_failCount > 3)
        {
                
            StartCoroutine(penalty());
        }

        m_lastSuccess = success;
    }

    IEnumerator penalty()
    {
        if (OnJamStart != null) OnJamStart();
        m_blocked = true;
        yield return new WaitForSeconds(penaltyLength);
        m_blocked = false;
        m_failCount = 0;
        if (OnJamEnd != null) OnJamEnd();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("ENEMY IN");
        if (other.tag == "Enemy")
        {
            
            addNote(other.GetComponent<Enemy>());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("ENEMY OUT");
        if (other.tag == "Enemy")
        {
            
            removeEnemy(other.GetComponent<Enemy>());
            m_health--;
            if (m_health < 0) m_health = 0;
            if (OnChangeHealth != null) OnChangeHealth(m_health);
        }
    }
 

    void OnDisable()
    {
        AudioInput.OnNoteStart -= AudioInput_OnNoteStart;
        AudioInput.OnNoteChange -= AudioInput_OnNoteStart;
    }
}
