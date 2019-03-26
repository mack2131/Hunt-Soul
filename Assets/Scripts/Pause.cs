using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

    public GameObject pausePanel;
    public GameObject settingsPanel;
    public bool pause;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && !pause)
            PauseOn();
        else if (Input.GetKeyUp(KeyCode.Escape) && pause)
            PauseOff();
	}

    void PauseOn()
    {
        pausePanel.transform.localScale = Vector3.one;
        Time.timeScale = 0;
        pause = true;
    }

    public void PauseOff()
    {
        pausePanel.transform.localScale = Vector3.zero;
        settingsPanel.transform.localScale = Vector3.zero;
        Time.timeScale = 1;
        pause = false;
    }
}
