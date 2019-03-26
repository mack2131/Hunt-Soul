using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LitJson;
using System.IO;

public class SettingsPanel : MonoBehaviour {

    public Slider volumeSlider;

    private float currentVolume;
    private string file;

	// Use this for initialization
	void Start ()
    {
        //SaveSettings();
        file = Application.dataPath + "//StreamingAssets" + "/settings.whatareyoulookingfor";
        LoadSettings();
        volumeSlider.onValueChanged.AddListener(delegate { SetVolumeValue(); });
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(settings.Settings);//создаем строку json из объекта
        File.WriteAllText(file, json);//сохраняем строку в файл
    }

    public void LoadSettings()
    {
        if (File.Exists(file))
        {
            string json = File.ReadAllText(file);//загружаем строку из файла
            settings.Settings = JsonUtility.FromJson<settings>(json);//преобразуем строку в объект

            /* загружаем настройки звука */
            volumeSlider.value = settings.Settings.volume;
            AudioListener.volume = volumeSlider.value;
        }
        else SaveSettings();
    }

    public void SetVolumeValue()
    {
        settings.Settings.volume = volumeSlider.value;
        AudioListener.volume = volumeSlider.value;
    }

    void OkButton()
    {
        SaveSettings();
        transform.localScale = Vector3.zero;
    }
}

public struct settings
{
    public static settings Settings = new settings();
    public int fullscreen;
    public float volume;
}
