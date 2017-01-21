using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed;
    public NOTE note;
    float m_velocity;

    public static float fullLength = 1000;
	// Use this for initialization
	void Start ()
    {
        GetComponent<TextMesh>().text = note.ToString();
        m_velocity = fullLength * speed * Time.fixedDeltaTime;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector3 position = transform.position;
        position.x -= m_velocity;
        transform.position = position;
	}

    
}
