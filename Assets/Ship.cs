using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour
{
    public int maxHealth;
    int m_health;
    Animator m_animator;

	// Use this for initialization
    void Start ()
    {
        m_health = maxHealth;
        
	}

    void OnEnable()
    {
        m_animator = GetComponent<Animator>();
    }

    public void playExplosionSound()
    {

    }

    public void setHealth(int value)
    {
        m_health = value;
        maxHealth = value;
    }

    public void arrive()
    {
        m_animator.Play("Ship_Arrive");
    }

    public void hasArrived()
    {
        Debug.Log("Has Arrived");
    }

    public void fire()
    {

    }
    
    public void explode()
    {

    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
