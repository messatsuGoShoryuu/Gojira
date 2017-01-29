using UnityEngine;
using System.Collections;


public class HealthBar : MonoBehaviour
{
    private static HealthBar m_singleton;
    public static HealthBar singleton
    {
        get
        {
            if (m_singleton == null) m_singleton = GameObject.FindObjectOfType<HealthBar>();
            return m_singleton;
        }
    }
    public Transform bar;
    private Vector3 m_scale;
    private Vector3 m_position;
    private Vector3 m_startingScale;
    private Vector3 m_startingPosition;
    // Use this for initialization
    void Start ()
    {
        m_scale = bar.transform.localScale;
        m_position = bar.transform.position;
        m_startingPosition = m_position;
        m_startingScale = m_scale;
	}

    public static void setHealth(float value)
    {
        singleton._setHealth(value);
       
    }

    void _setHealth(float value)
    {
        m_scale.x = m_startingScale.x * value;
        m_position.x = m_startingPosition.x - (m_startingScale.x - m_scale.x)/2.0f;
        updateTransform();
    }

    void updateTransform()
    {
        bar.transform.localScale = m_scale;
        bar.transform.position = m_position;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
