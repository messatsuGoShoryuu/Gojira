using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        GameObject.Destroy(this.gameObject,lifetime);
	}

    public float speed;
    public float lifetime;
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        transform.Translate(0.0f, speed, 0.0f);
	}
}
