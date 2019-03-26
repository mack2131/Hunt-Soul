using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour {

    public GameObject moon;
    public Text timeDisplay;
    public bool isDay;
    public float howMuschMinutesInGameHour;
    public int degreePerHour;
    public float degreePerMinutes;

    private int seconds;
    public int gameMinutes;
    public int gameHours;
    private int days;

	// Use this for initialization
	void Start ()
    {
        ShowTime();
        SetSunMoon();
        StartCoroutine(Clock());
    }
	
	// Update is called once per frame
	void Update ()
    {
        MoveSunMoon();
        //Camera.main.transform.LookAt(moon.transform.position);
	}

    IEnumerator Clock()
    {
        while (true)
        {
            //я знаю, что такое решение очень плохое
            //нужно сделать одну инт переменную, в которую записывать секунды, а потом уже считать часы минуты и т.д.
            gameMinutes++;
            seconds++;
            if(gameMinutes == 60)
            {
                gameMinutes = 0;
                gameHours++;
                if(gameHours == 24)
                {
                    gameHours = 0;
                    seconds = 0;
                    days++;
                }
            }
            ShowTime();
            yield return new WaitForSeconds(howMuschMinutesInGameHour/*1f*/);
        }
    }

    void ShowTime()
    {
        timeDisplay.text = gameHours + " : " + gameMinutes;
    }

    void SetSunMoon()
    {
        float rotX = -90;
        transform.rotation = Quaternion.Euler(rotX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        rotX = transform.rotation.eulerAngles.x + (gameHours * degreePerHour) + (gameMinutes * degreePerMinutes);
        transform.rotation = Quaternion.Euler(rotX, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }

    void MoveSunMoon()
    {
        if(gameHours < 12 && gameHours >= 0)
        {
            isDay = false;
        }
        else
        {
            isDay = true;
        }
        transform.RotateAround(Vector3.zero, transform.right, (degreePerMinutes * Time.deltaTime) * (1 / howMuschMinutesInGameHour));
    }
}
