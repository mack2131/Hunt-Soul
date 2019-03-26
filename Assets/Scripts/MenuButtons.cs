using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public int buttonId;
    public Color color;

    // Use this for initialization
    void Start ()
    {
        switch (buttonId)
        {
            case 0://продолжить
                {
                    GetComponent<Button>().onClick.AddListener(Continue);//вызываем обработчик кнопки Выход
                    color.a = 0;
                    GetComponent<Image>().color = color;
                    break;
                }
            case 1://play
                {
                    if(SceneManager.GetActiveScene().name == "Main Menu")
                        GetComponent<Button>().onClick.AddListener(PlayMenu);//вызываем обработчик кнопки играть
                    else GetComponent<Button>().onClick.AddListener(PlayNotMenu);//вызываем обработчик кнопки играть
                    color.a = 0;
                    GetComponent<Image>().color = color;
                    break;
                }
            case 2://settings
                {
                    GetComponent<Button>().onClick.AddListener(Settings);//вызываем обработчик кнопки играть
                    color.a = 0;
                    GetComponent<Image>().color = color;
                    break;
                }
            case 3://титры
                {
                    GetComponent<Button>().onClick.AddListener(Credits);//вызываем обработчик кнопки Выход
                    color.a = 0;
                    GetComponent<Image>().color = color;
                    break;
                }
            case 4://exit
                {
                    GetComponent<Button>().onClick.AddListener(ExitGame);//вызываем обработчик кнопки Выход
                    color.a = 0;
                    GetComponent<Image>().color = color;
                    break;
                }
            case 5://копка готова в панели меню настроек
                {
                    GetComponent<Button>().onClick.AddListener(OkButton);
                    color.a = 0;
                    GetComponent<Image>().color = color;
                    break;
                }
            case 6:
                {
                    GetComponent<Button>().onClick.AddListener(ReturnToMM);
                    color.a = 0;
                    GetComponent<Image>().color = color;
                    break;
                }
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        color.a = 255;
        GetComponent<Image>().color = color;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        color.a = 0;
        GetComponent<Image>().color = color;
    }

    void Continue()
    {

    }

    void PlayMenu()//если кнопка играть в сцене меню
    {
        //загружаем сцену с уровнем
        SceneManager.LoadScene("Hub");
    }

    void PlayNotMenu()//если кнопка играть не в сцене меню
    {
        transform.parent.GetComponent<Pause>().PauseOff();
    }

    void Settings()
    {
        GameObject.FindObjectOfType<SettingsPanel>().transform.localScale = Vector3.one;
    }

    void Credits()
    {

    }

    void OkButton()
    {
        transform.parent.GetComponent<SettingsPanel>().SaveSettings();
        transform.parent.transform.localScale = Vector3.zero;
    }

    void ReturnToMM()
    {
        GameObject.FindObjectOfType<SettingsPanel>().SaveSettings();
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }

    void ExitGame()//функция, которая вызывается при нажитии кнопки выход
    {
        GameObject.FindObjectOfType<SettingsPanel>().SaveSettings();
        //выходим из игры
        Application.Quit();
    }
}
