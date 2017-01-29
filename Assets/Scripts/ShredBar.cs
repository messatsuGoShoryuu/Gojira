using UnityEngine;
using System.Collections;

public class ShredBar : MonoBehaviour
{
    public Transform bar;
    private Vector3 m_scale;
    private Vector3 m_position;
    private Vector3 m_startingScale;
    private Vector3 m_startingPosition;
    // Use this for initialization
    void Start()
    {
        m_scale = bar.transform.localScale;
        m_position = bar.transform.position;
        m_startingPosition = m_position;
        m_startingScale = m_scale;
    }


    public void setHealth(float value)
    {
        Debug.Log("value = " + value);
        m_scale.x = m_startingScale.x * value;
        m_position.x = m_startingPosition.x - (m_startingScale.x - m_scale.x) / 2.0f;
        updateTransform();
    }

    void updateTransform()
    {
        bar.transform.localScale = m_scale;
        bar.transform.position = m_position;
    }
    
}
