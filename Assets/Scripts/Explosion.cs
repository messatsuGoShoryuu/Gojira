using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	 
	}

    public void destroy()
    {
        GameObject.Destroy(this.gameObject);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
