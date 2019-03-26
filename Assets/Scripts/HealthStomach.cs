using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class HealthStomach : MonoBehaviour {

    public GameObject prefabDonut;
    public Transform donutStorage;
    public GameObject player;

    private int donutCounter = 0;
    private float counter = 0;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindObjectOfType<Player>().gameObject;
        IHungry();
    }
	
	// Update is called once per frame
	void Update ()
    {

        /*if (Input.GetKeyUp(KeyCode.P))
            SavePoses();*/
	}

    void IHungry()
    {
        string json = File.ReadAllText(Application.dataPath + "//StreamingAssets" + "/startPos.whatareyoulookingfor");//загружаем строку из файла
        DonutPosition posi = new DonutPosition();
        int count = 0;
        List<string> l = new List<string>();
        for (int j = 0; j < player.GetComponent<Player>().currentHealth; j++)
        {
            string text = "";
            for (int i = count; i < json.Length; i++)
            {
                if (json[i] == '}')
                {
                    text += json[i];
                    count = i + 1;
                    l.Add(text);
                    GameObject donut = Instantiate(prefabDonut, donutStorage);
                    posi = JsonUtility.FromJson<DonutPosition>(text);
                    donut.transform.localPosition = new Vector3(posi.x, posi.y, posi.z);
                    break;
                }
                else text+= json[i]; 
            }
        }        
    }

    public void IWantToEat()
    {
        if (donutCounter != player.GetComponent<Player>().currentHealth)
        {
            counter += Time.deltaTime;
            if (counter > 0.15f)
            {
                counter = 0;
                GameObject donut = Instantiate(prefabDonut, donutStorage);
                donutCounter++;
            }
        }
        else
        {
            //StreamingAssets
        }
    }

    public void IWasteMyFood(int damage)
    {
        if (donutStorage.childCount - damage >= 0)
        {
            for (int i = 0; i < damage; i++)
            {
                Destroy(donutStorage.GetChild(i).gameObject);
            }
        }
        else if (donutStorage.childCount - damage < 0)
        {
            for(int i = 0; i < donutStorage.childCount; i++)
            {
                Destroy(donutStorage.GetChild(i).gameObject);
            }
        }
    }

    void SavePoses()
    {
        //DonutPosition[] donutPoses = new DonutPosition[300];
        //DonutPosition.donutPoses[0].x = 0;
        DonutPosition.donutPoses = new DonutPosition[300];
        string json = "";
        for (int i = 0; i < 300; i++)
        {
            DonutPosition.donutPoses[i] = new DonutPosition();
        }
        for (int i = 0; i < donutStorage.childCount; i++)
        {
            DonutPosition.donutPoses[i].i = i;
            DonutPosition.donutPoses[i].x = donutStorage.GetChild(i).transform.localPosition.x;
            DonutPosition.donutPoses[i].y = donutStorage.GetChild(i).transform.localPosition.y;
            DonutPosition.donutPoses[i].z = donutStorage.GetChild(i).transform.localPosition.z;
            json += JsonUtility.ToJson(DonutPosition.donutPoses[i]);//создаем строку json из объекта savedGame класса SaveData
        }
        //string json = JsonUtility.ToJson(DonutPosition.donutPoses[0]);//создаем строку json из объекта savedGame класса SaveData
        File.WriteAllText(Application.dataPath + "//StreamingAssets" + "/startPos.whatareyoulookingfor", json);//сохраняем строку в файл
    }

    

}
[Serializable]
public struct DonutPosition
{
    public static DonutPosition[] donutPoses = new DonutPosition[300];
    public int i;
    public float x;
    public float y;
    public float z;
}

