using UnityEngine;
using System.Collections;

public class HealthDisplayer : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        m_textMesh = GetComponent<TextMesh>();
        m_textMesh.text = "100";
    }

    TextMesh m_textMesh;
	
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
        m_textMesh.text = health.ToString();
    }

    void OnDisable()
    {
        DamageZone.OnChangeHealth -= DamageZone_OnChangeHealth;
    }
}
