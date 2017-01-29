using UnityEngine;
using System.Collections;

public class HealthDisplayer : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        
    }

	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnEnable()
    {
        DamageZone.OnChangeHealth += DamageZone_OnChangeHealth;
    }

    private void DamageZone_OnChangeHealth(int health)
    {
        float health_ = health / 10.0f;
        Debug.Log(health_);
        HealthBar.setHealth((float)health / 10.0f);
    }

    void OnDisable()
    {
        DamageZone.OnChangeHealth -= DamageZone_OnChangeHealth;
    }
}
