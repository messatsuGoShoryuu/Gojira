using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour
{
    public int maxHealth;
    int m_health;
    Animator m_animator;
    bool m_fire = false;

    public GameObject topLeft;
    public GameObject bottomRight;
    public GameObject explosionObject;
    

    public GameObject cannonBall;

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
        m_animator.Play("Ship_Idle");
    }

    public void fire()
    {
        m_fire = true;
       
        GameObject.Instantiate(cannonBall,transform.position,Quaternion.identity);
    }

    public void explode()
    {
        m_fire = false;
        StartCoroutine(explosion());
    }


    IEnumerator explosion()
    {
        yield return new WaitForSeconds(0.2f);
        spawnExplosion();
        yield return new WaitForSeconds(0.2f);
        spawnExplosion();
        yield return new WaitForSeconds(0.2f);
        spawnExplosion();
        yield return new WaitForSeconds(0.2f);
        spawnExplosion();
        yield return new WaitForSeconds(0.2f);
        spawnExplosion();
        yield return new WaitForSeconds(0.2f);
        spawnExplosion();
    }

    void spawnExplosion()
    {
        Vector3 position = transform.position;
        position.z -= 0.1f;
        position.x = Random.Range(topLeft.transform.position.x, bottomRight.transform.position.x);
        position.y = Random.Range(topLeft.transform.position.y, bottomRight.transform.position.y);
        GameObject.Instantiate(explosionObject, position, Quaternion.identity);
    }

	// Update is called once per frame
	void Update ()
    {
	
	}
}
