using UnityEngine;
using System.Collections;

public class NoteSpawner : MonoBehaviour
{
    public GameObject[] spawnPoints;
    bool[] m_possibleNotes;
    TextMesh m_textMesh;
    int m_enabledNoteCount = 0;
    NOTE[] m_noteBank;

    public Enemy enemy;
    public float currentSpeed;

    public static int enemyCount;
	// Use this for initialization
	void Start ()
    {
        m_noteBank = new NOTE[12];
        m_textMesh = GetComponent<TextMesh>();
        m_possibleNotes = new bool[12];
        for (int i = 0; i < 12; i++) m_possibleNotes[i] = false;
        setPossible(NOTE.A,true);
        setPossible(NOTE.C, true);
        setPossible(NOTE.E, true);
        setPossible(NOTE.D, true);
        setPossible(NOTE.B, true);
        refresh();
	}


    public void setPossible(NOTE note, bool value)
    {
        m_possibleNotes[(int)note] = value;
        
    }

    public void refresh()
    {
        m_enabledNoteCount = 0;
        for (int i = 0; i < 12; i++)
        {
            if (m_possibleNotes[i])
            {
                m_noteBank[m_enabledNoteCount] = (NOTE)i;
                m_enabledNoteCount++;
            }
        }
    }

    public void spawn()
    {
        if (DamageZone.gameOver) return;
        int spawningPoint = Random.Range(0, spawnPoints.Length);
        int noteIndex = Random.Range(0, m_enabledNoteCount);
        Enemy e = GameObject.Instantiate(enemy, spawnPoints[spawningPoint].transform.position, Quaternion.identity) as Enemy;
        e.speed = currentSpeed;
        e.note = m_noteBank[noteIndex];
        enemyCount++;
    }
	
	// Update is called once per frame
	void Update ()
    {
       
	}
}
