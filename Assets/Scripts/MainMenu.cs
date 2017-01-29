using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    int m_choice;
    public GameObject arrow_;
    public GameObject play_;
    public GameObject instructions_;
    public GameObject exit_;
    public GameObject inputSelect_;

    public AudioClip highlightClip;
    public AudioClip selectClip;

    public GameObject inputArrow;
    int m_inputSelector;
    int m_maxInputSelector;
    int m_currentInputChoice;

    AudioSource m_source;
    bool m_block = false;
    bool m_instructionMode = false;

    public GameObject[] deviceSlotLocations;
    public TextMesh[] deviceSlotTexts;
    public GameObject inputMenuExitLocation;

    private delegate void UpdateFunction();

    UpdateFunction m_updateFunction;
    
	// Use this for initialization
	void Start ()
    {
        m_source = GetComponent<AudioSource>();
        m_updateFunction = mainMenu;
        GameInfo.currentDevice = deviceSlotTexts[0].text;
        GameInfo.singleton.scoreText.gameObject.SetActive(false);
    }

    void toMainMenu()
    {
        m_updateFunction = mainMenu;
        Camera.main.transform.position = new Vector3(0.0f, 0.0f, -10.0f);
    }

    void instructions()
    {
        if (Input.anyKeyDown)
        {
            toMainMenu();
        }
    }

    void mainMenu()
    {
        if (m_block) return;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_choice--;
            m_source.clip = highlightClip;
            m_source.loop = false;
            m_source.Play();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_source.clip = highlightClip;
            m_source.loop = false;
            m_source.Play();
            m_choice++;
        }
        m_choice = (m_choice + 4) % 4;
        if (m_choice == 0) arrow_.transform.position = play_.transform.position;
        if (m_choice == 1) arrow_.transform.position = instructions_.transform.position;
        if (m_choice == 2) arrow_.transform.position = inputSelect_.transform.position;
        if (m_choice == 3) arrow_.transform.position = exit_.transform.position;
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            
            switch (m_choice)
            {
                case 0:
                    m_block = true;
                    StartCoroutine(lateLoad());
                    break;
                case 1:
                    Camera.main.transform.Translate(20.0f, 0.0f, 0.0f);
                    m_updateFunction = instructions;
                    break;
                case 2:
                    inputSelectInit();
                    Camera.main.transform.Translate(40.0f, 0.0f, 0.0f);
                    m_updateFunction = inputSelect;
                    break;
                case 3:
                    Application.Quit();
                    break;
            }

            m_source.clip = selectClip;
            m_source.loop = false;
            m_source.Play();
        }
    }
    
    void inputSelectInit()
    {
        string[] devices = GameInfo.devices;
        int count = devices.Length;
        if (count > 5) count = 5;
        for(int i = 0; i<count;i++)
        {
            deviceSlotTexts[i+1].text = devices[i];
            if (deviceSlotTexts[i+1].text.Length > 64) deviceSlotTexts[i+1].text = deviceSlotTexts[i].text.Remove(64);
        }
        m_maxInputSelector = count + 1;
        deviceSlotLocations[m_maxInputSelector] = deviceSlotLocations[deviceSlotLocations.Length - 1];
        deviceSlotTexts[m_currentInputChoice].color = Color.yellow;
    }
    void inputSelect()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            m_inputSelector--;
            m_source.clip = highlightClip;
            m_source.loop = false;
            m_source.Play();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            m_source.clip = highlightClip;
            m_source.loop = false;
            m_source.Play();
            m_inputSelector++;
        }

        Debug.Log("m_inputSelector = "  + m_inputSelector);

        m_inputSelector = (m_inputSelector + m_maxInputSelector + 1) % (m_maxInputSelector + 1);

        Vector3 p = deviceSlotLocations[m_inputSelector].transform.position;
        inputArrow.transform.position = p;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            m_source.clip = selectClip;
            m_source.loop = false;
            m_source.Play();
            if (m_inputSelector < m_maxInputSelector)
            {
                deviceSlotTexts[m_currentInputChoice].color = Color.white;
                m_currentInputChoice = m_inputSelector;
                deviceSlotTexts[m_currentInputChoice].color = Color.yellow;
                if (m_currentInputChoice > 0)
                    GameInfo.currentDevice = GameInfo.devices[m_currentInputChoice - 1];
                else GameInfo.currentDevice = deviceSlotTexts[0].text;
            }
            else toMainMenu();
        }
    }

    IEnumerator lateLoad()
    {
        yield return new WaitForSeconds(0.5f);
        Application.LoadLevel("Intro");
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_updateFunction();
	}
}
