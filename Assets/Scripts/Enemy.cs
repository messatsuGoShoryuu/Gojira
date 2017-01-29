using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed;
    public NOTE note;
    float m_velocity;
    int m_lives = 0;

    float m_lifeTime = 20.0f;
    float m_startTime;


    AudioSource m_source;
    public AudioClip dieClip;
    public AudioClip powerupClip;
    public static float fullLength = 1000;

    SpriteRenderer m_spriteRenderer;
    public GameObject powerUpSpawnLocation;
    public GameObject powerUp;
	// Use this for initialization
	void Start ()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_source = GetComponent<AudioSource>();
        m_startTime = Time.time;
        m_velocity = fullLength * speed * Time.fixedDeltaTime;
        findAnimation();
        
	}

    void findAnimation()
    {
        switch(note)
        {
            case NOTE.A: GetComponent<Animator>().Play("Surfer_A");
                break;
            case NOTE.B:
                GetComponent<Animator>().Play("Surfer_B");
                break;
            case NOTE.C:
                GetComponent<Animator>().Play("Surfer_C");
                break;
            case NOTE.D:
                GetComponent<Animator>().Play("Surfer_D");
                break;
            case NOTE.E:
                GetComponent<Animator>().Play("Surfer_E");
                break;

        }
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (DamageZone.gameOver) return;
        Vector3 position = transform.position;
        position.x -= m_velocity;
        transform.position = position;
        if (Time.time - m_startTime > m_lifeTime) GameObject.Destroy(this.gameObject);
	}

    public void reinforce()
    {
        m_lives++;
        m_source.clip = powerupClip;
        m_source.Play();
        GameObject p = GameObject.Instantiate(powerUp, powerUpSpawnLocation.transform.position, Quaternion.identity) as GameObject;
        p.transform.localScale = Vector3.one;
    }

    

    public void die()
    {

        if (m_lives == 0)
        {
            m_source.clip = dieClip;
            m_source.Play();
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(blink());
            GameObject.Destroy(this.gameObject, dieClip.length + 0.3f);
        }
        else m_lives--;
    }

    IEnumerator blink()
    {
        yield return new WaitForSeconds(0.05f);
        m_spriteRenderer.enabled = !m_spriteRenderer.enabled;
        StartCoroutine(blink());
    }
}
