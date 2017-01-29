using UnityEngine;
using System.Collections;

public class GameInfo : MonoBehaviour
{
    private static GameInfo m_singleton;
    public static GameInfo singleton
    {
        get
        {
            if (m_singleton == null) m_singleton = GameObject.FindObjectOfType<GameInfo>();
            return m_singleton;
        }
    }

    public int score;
    public TextMesh scoreText;
    public static string[] devices;
    public static string currentDevice;
    
    public static int getScore()
    {
        return singleton.score;
    }

    public void getDeviceNames()
    {
        devices = Microphone.devices;
    }

    public static void setScore(int value)
    {
        singleton.score = value;
        singleton.scoreText.text = value.ToString();
    }

    // Use this for initialization
    void Start ()
    {
        if (GameObject.FindObjectsOfType<GameInfo>().Length > 1) GameObject.Destroy(this.gameObject);
        else
        {
            getDeviceNames();
            DontDestroyOnLoad(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Debug.Log("current device: " + currentDevice);
	}
}
