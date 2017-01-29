using UnityEngine;
using System.Collections;

public class Gojira : MonoBehaviour
{
    DamageZone m_damageZone;
    public Wave wave;
    float[] m_startingY;
    public GameObject spawnPosition;
    Animator m_animator;
    public AudioClip takeDamageClip;
    AudioSource m_source;
    public GameObject laneSelector;
    int m_laneSelector = 2;
    public bool keyboardControls;

	// Use this for initialization
	void Start ()
    {
        m_source = GetComponent<AudioSource>();
        m_damageZone = GameObject.FindObjectOfType<DamageZone>();
        m_startingY = new float[4];
        NoteSpawner spawner = GameObject.FindObjectOfType<NoteSpawner>();
        for (int i = 0; i<4;i++)
        {
            m_startingY[i] = spawner.spawnPoints[i].transform.position.y;
        }
        m_animator = GetComponent<Animator>();
        play();
	}

    

    Enemy findClosest(NOTE note)
    {
        Enemy[] enemies = m_damageZone.getEnemies();
        for(int i = 0; i< enemies.Length;i++)
        {
            if (enemies[i].note == note) return enemies[i];
        }
        return null;
    }

    public void spawn(NOTE note)
    {
        if (DamageZone.gameOver) return;
        Vector3 position = Vector3.zero;
        position.x = spawnPosition.transform.position.x;
        if (!keyboardControls)
        {
            Enemy e = findClosest(note);

            
            if (e == null)
            {
                int index = Random.Range(0, 4);
                position.y = m_startingY[index];
            }
            else position.y = e.transform.position.y;
        }
        else position.y = m_startingY[m_laneSelector];

        Wave w = GameObject.Instantiate(wave, position, Quaternion.identity) as Wave;
        w.setNote(note);
    }
    
    public void shred()
    {
        if (DamageZone.gameOver) return;
        m_animator.Play("Shred");
    }

    public void takeDamage()
    {
        if (DamageZone.gameOver) return;
        m_source.clip = takeDamageClip;
        m_source.Play();
        m_animator.Play("Damage");
    }

    public void play()
    {
        if (DamageZone.gameOver) return;
        m_animator.Play("Play");
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_laneSelector--;
            if (m_laneSelector < 0) m_laneSelector = 0;
        }
        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_laneSelector++;
            if (m_laneSelector >= m_startingY.Length) m_laneSelector = m_startingY.Length - 1;
        }
        if (DamageZone.gameOver) return;

        Vector3 position = laneSelector.transform.position;
        position.y = m_startingY[m_laneSelector];
        laneSelector.transform.position = position;
    }
}
