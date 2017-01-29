using UnityEngine;
using System.Collections;

public class JammedDisplayer : MonoBehaviour
{
    TextMesh m_textMesh;
    MeshRenderer m_renderer;
	// Use this for initialization
	void Start ()
    {
        m_textMesh = GetComponent<TextMesh>();
        m_renderer = GetComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnEnable()
    {
        DamageZone.OnJamStart += DamageZone_OnJamStart;
        DamageZone.OnJamEnd += DamageZone_OnJamEnd;
    }

    private void DamageZone_OnJamEnd()
    {
        m_renderer.enabled = false;
    }

    private void DamageZone_OnJamStart()
    {
        m_renderer.enabled = true;
    }

    void OnDisable()
    {
        DamageZone.OnJamStart -= DamageZone_OnJamStart;
        DamageZone.OnJamEnd -= DamageZone_OnJamEnd;
    }
}
