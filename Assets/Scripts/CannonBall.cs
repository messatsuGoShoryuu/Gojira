using UnityEngine;
using System.Collections;

public class CannonBall : MonoBehaviour
{
    Vector3 m_destination;
    public float speed;
    public GameObject explosion;
	// Use this for initialization
	void Start ()
    {
        m_destination = FindObjectOfType<Gojira>().transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_destination, speed);
        {
            if(m_destination == transform.position)
            {
                GameObject.Instantiate(explosion, transform.position, Quaternion.identity);
                GameObject.Destroy(this.gameObject);
            }
        }
	}
}
