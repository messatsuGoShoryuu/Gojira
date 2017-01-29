using UnityEngine;
using System.Collections;




public class SpawnScheduler : MonoBehaviour
{
    NoteSpawner m_noteSpawner;
    int m_spawnedUnits;
    int m_surferCount;
    int m_currentWave = 0;
    float m_spawnFrequency;
    bool m_waitForShred = false;
    int m_nextShred = 5;

    public delegate void StartShred();
    public static event StartShred OnStartShred;

    

	// Use this for initialization
	void Start ()
    {
        m_noteSpawner = FindObjectOfType<NoteSpawner>();
        
        calculateNextWave();
        StartCoroutine(spawn());
    }

    public float spawnFrequency;

    IEnumerator spawn()
    {
        Debug.Log("spawn" + m_spawnFrequency);

        yield return new WaitForSeconds(m_spawnFrequency);
        Debug.Log("spawn coroutine");
        m_noteSpawner.spawn();
        m_spawnedUnits++;
        if (m_spawnedUnits >= m_surferCount) calculateNextWave();
        if (m_currentWave >= m_nextShred)
        {
            Debug.Log("wait for shred");
            m_waitForShred = true;
        }
        else StartCoroutine(spawn());

    }

    void waitForShred()
    {
        if(m_waitForShred)
        {
            if (GameObject.FindObjectsOfType<Enemy>().Length == 0)
            {
                m_waitForShred = false;
                startShred();
            }
        }
        
    }
    
    void OnEnable()
    {
        Shred.OnEndShred += endShred;
    }

    void OnDisable()
    {
        Shred.OnEndShred -= endShred;
    }

    void startShred()
    {
        m_nextShred += 5;
        if (OnStartShred != null) OnStartShred();
    }

    void endShred()
    {
        Debug.Log("GAME OVER = " + DamageZone.gameOver);
        StartCoroutine(spawn());
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (DamageZone.gameOver)
        {
            StopAllCoroutines();
            return;
        }
        waitForShred();
	}

    void calculateNextWave()
    {
        m_currentWave++;
        m_spawnedUnits = 0;
        float waveTime = Mathf.Floor(m_currentWave * 0.75f) * 2.0f + 10.0f;
        m_surferCount = Mathf.FloorToInt(m_currentWave * 0.66f) * 2 + 3;

        float inverseCount = 1.0f / m_surferCount;
        m_spawnFrequency = waveTime * inverseCount * 0.45f;
    }
}
