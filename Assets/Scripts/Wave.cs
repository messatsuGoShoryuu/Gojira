using UnityEngine;
using System.Collections;

public class Wave : MonoBehaviour
{
    public NOTE note;
    public float speed;
    float m_lifeTime = 10.0f;
    float m_start = 0.0f;
    public TextMesh textMesh;
	// Use this for initialization
	void Start ()
    {
        m_start = Time.time;
	}

    public void setNote(NOTE note)
    {
        this.note = note;
        textMesh.text = getNoteName();
    }
    string getNoteName()
    {
        switch(note)
        {
            case NOTE.A: return "A";
            case NOTE.B: return "B";
            case NOTE.C: return "C";
            case NOTE.D: return "D";
            case NOTE.E: return "E";
            case NOTE.F: return "F";
            case NOTE.G: return "G";
            case NOTE.As: return "A#";
            case NOTE.Cs: return "C#";
            case NOTE.Ds: return "D#";
            case NOTE.Fs: return "F#";
            case NOTE.Gs: return "G#";
            default: return null;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (DamageZone.gameOver) return;
        transform.Translate(speed, 0, 0);
        if (Time.time - m_start > m_lifeTime) GameObject.Destroy(gameObject);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            Enemy e = other.GetComponent<Enemy>();
            if (e.note == note)
            {
                e.die();
                GameInfo.setScore(GameInfo.getScore() + 1);
            }
            else e.reinforce();
            {
                GameObject.Destroy(gameObject);
            }
            
            
        }
    }
}
