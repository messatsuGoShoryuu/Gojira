using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CompareEnemies : IComparer<Enemy>
{

    public int Compare(Enemy a, Enemy b)
    {
        if (a.transform.position.x > b.transform.position.x) return 1;
        return -1;
    }

}
public class DamageZone : MonoBehaviour
{
    static bool m_blocked = false;
    public float penaltyLength;
     int m_health = 10;
    public int m_failCount = 0;

    public delegate void ChangeHealth(int health);
    public static event ChangeHealth OnChangeHealth;

    public delegate void JamStart();
    public static event JamStart OnJamStart;

    public delegate void JamEnd();
    public static event JamEnd OnJamEnd;

    public delegate void GameOver();
    public static event GameOver OnGameOver;

    public static bool gameOver = false;
    public AudioSource backTrack;
    public AudioClip gameOverClip;

    public int shredTime;
    bool m_lastSuccess = false;
    int m_consecutiveSuccess = 0;

    float m_lastSpawnTime;

    private Gojira m_gojira;

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
        m_gojira = GameObject.FindObjectOfType<Gojira>();
        gameOver = false;
        GetComponent<SpriteRenderer>().enabled = false;
        
        GameInfo.singleton.scoreText.gameObject.SetActive(true);
        GameInfo.setScore(0);
    }

    Enemy[] m_availableNotes;
    int m_noteCount;

    public float health
    {
        get
        {
            return (float)m_health;
        }
    }
	
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
       GameObject.Destroy(enemy.gameObject);
    }

    

    public Enemy[] getEnemies()
    {
        Enemy[] result = GameObject.FindObjectsOfType<Enemy>();
        List<Enemy> list = new List<Enemy>();
        for(int i = 0; i<result.Length;i++)
        {
            list.Add(result[i]);
        }

        IComparer<Enemy> comparer = new CompareEnemies();
        list.Sort(comparer);
        
        return list.ToArray();
    }

    void OnEnable()
    {
            AudioInput.OnNoteStart += AudioInput_OnNoteStart;
            AudioInput.OnNoteChange += AudioInput_OnNoteStart;
            Shred.OnEndShred += endShred;
    }
    
    void endShred()
    {
        float damage = Shred.damage;
        if (damage > 1.0f) damage = 1.0f;
        damage = 1.0f - damage;
        damage *= 10.0f;

        m_health -= Mathf.CeilToInt(damage);
        m_gojira.takeDamage();
        if (OnChangeHealth != null) OnChangeHealth(m_health);
        if (!checkForGameOver())
            GameInfo.setScore(GameInfo.getScore() + 10);

    }

    private void AudioInput_OnNoteEnd(NoteData note)
    {
        throw new System.NotImplementedException();
    }

    private void AudioInput_OnNoteStart(NoteData note)
    {
        if (m_blocked || Shred.isShredding || gameOver) return;
        if(Time.time - m_lastSpawnTime > 0.01f)m_gojira.spawn(note.note);
        m_lastSpawnTime = Time.time;
        /*
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
        */
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
        if (other.tag == "Enemy")
        {
            
         //   addNote(other.GetComponent<Enemy>());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (gameOver) return;
        if (other.tag == "Enemy")
        {
            removeEnemy(other.GetComponent<Enemy>());
            m_health--;
            checkForGameOver();
            if (OnChangeHealth != null) OnChangeHealth(m_health);
            m_gojira.takeDamage();
        }
    }

    bool checkForGameOver()
    {
        if (m_health <= 0)
        {
            m_health = 0;
            gameOver = true;
            StartCoroutine(toMainMenu());
            GetComponent<SpriteRenderer>().enabled = true;
            backTrack.clip = gameOverClip;
            backTrack.loop = false;
            backTrack.Play();
            return true;
        }
        return false;
    }
 
    IEnumerator toMainMenu()
    {
        yield return new WaitForSeconds(5.0f);
        Application.LoadLevel("MainMenu");
    }
    
    void OnDisable()
    {
        AudioInput.OnNoteStart -= AudioInput_OnNoteStart;
        AudioInput.OnNoteChange -= AudioInput_OnNoteStart;
        Shred.OnEndShred -= endShred;
    }
}
