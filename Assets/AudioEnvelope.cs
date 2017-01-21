using UnityEngine;
using System.Collections;

public class AudioEnvelope : MonoBehaviour
{
    public Vector2 attack;
    public Vector2 decay;
    public Vector2 sustain;
    public Vector2 release;

    private Vector2[] m_goals;
    int m_currentGoal = 0;

    public float m_coefficient = 1.0f;
    float m_lastPoint;

    public Vector2 m_gain;
    private float m_startTime;
    private float m_lastTime;

    bool m_awake = false;
	// Use this for initialization
    void Start ()
    {
        m_goals = new Vector2[4];
        m_goals[0] = attack;
        m_goals[1] = decay;
        m_goals[2] = sustain;
        m_goals[3] = release;

       

        
    }

    public float gain()
    {
        return m_gain.y;
    }

    

    public void play()
    {
        m_gain.x = 0;
        m_gain.y = 0;
        m_currentGoal = 0;
        m_startTime = Time.time;
        m_awake = true;
        m_lastPoint = 0.0f;
        m_lastTime = 0.0f;
        m_coefficient = attack.y / attack.x;
    }

    void changeGoal()
    {
        if (m_currentGoal >= 3)
        {
            m_currentGoal = 4;
            return;
        }
        Vector2 slope = m_goals[m_currentGoal + 1] - m_goals[m_currentGoal];
        m_coefficient = slope.y / slope.x;
        m_currentGoal++;
        m_lastPoint = m_gain.y;
        m_lastTime = m_gain.x;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!m_awake) return;
        m_gain.x = Time.time - m_startTime;
        if (m_currentGoal < 4 && m_gain.x > m_goals[m_currentGoal].x) changeGoal();
        m_gain.y = m_lastPoint + (m_gain.x - m_lastTime) * m_coefficient;
        if (m_gain.y < 0) m_gain.y = 0;
	}
}
