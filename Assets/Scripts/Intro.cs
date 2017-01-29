using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour
{
    public TextMesh gojira;
    public TextMesh rudy;

    public Color alternateColor;
    public Color basicColor;
    bool m_turn = false;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(intro());
        gojira.text = "";
        rudy.text = "";
	}


    void swapColors()
    {
        if (m_turn)
        {
            rudy.GetComponent<MeshRenderer>().material.color = alternateColor;
            gojira.GetComponent<MeshRenderer>().material.color = basicColor;
        }
        else
        {
            rudy.GetComponent<MeshRenderer>().material.color = basicColor;
            gojira.GetComponent<MeshRenderer>().material.color = alternateColor;
        }

        m_turn = !m_turn;
    }

	IEnumerator intro()
    {
        yield return new WaitForSeconds(1.0f);
        rudy.text = "Gojira-samaaaa!";
        swapColors();
        yield return new WaitForSeconds(3.0f);
        gojira.text = "Hey Rudi-kun, what's the problem?";
        swapColors();
        yield return new WaitForSeconds(3.0f);
        rudy.text = "We need your help Gojira-sama!";
        swapColors();
        yield return new WaitForSeconds(3.0f);
        gojira.text = "By Neptune s beard, my good boy, what is going on?";
        swapColors();
        yield return new WaitForSeconds(3.0f);
        swapColors();
        rudy.text = "The freedom, innovation, and economic opportunity \nthat the constitutional democracy \nof NoShitlandia enables is in jeopardy.";
        yield return new WaitForSeconds(4.0f);
        rudy.text = "Ravers are coming!They are proposing a bill that \nwill make everyone listen only to technos music!";
        
        yield return new WaitForSeconds(4.0f);
        gojira.text = "To the congress, then!";
        swapColors();
        yield return new WaitForSeconds(3.0f);
        rudy.text = "No... They are coming from sea...";
        swapColors();
        yield return new WaitForSeconds(3.0f);
        swapColors();
        gojira.text = "Oh, it has come to this...";
        yield return new WaitForSeconds(2.0f);
        gojira.text = "but in times of universal turmoil, \nthere is only one solution";
        yield return new WaitForSeconds(4.0f);
        gojira.text = "Shredding like there is no tomorrow...";
        yield return new WaitForSeconds(5.0f);
        Application.LoadLevel("GamePlay");




    }
	// Update is called once per frame
	void Update ()
    {
	    if(Input.anyKeyDown)
        {
            Application.LoadLevel("GamePlay");
        }
	}
}
