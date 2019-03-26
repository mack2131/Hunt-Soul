using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class ItemDatabase : MonoBehaviour {
    [SerializeField]
    public List<Item> database = new List<Item>();
    public JsonData jData;

	// Use this for initialization
	void Start ()
    {
        jData = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "//StreamingAssets/items.whatareyoulookingfor"));
        BuildDatabase();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void BuildDatabase()
    {
        for(int i = 0; i <jData.Count; i++)
        {
            database.Add(new Item((int)jData[i]["id"],
                                   jData[i]["type"].ToString(),
                                   jData[i]["title"].ToString(),
                                   (int)jData[i]["price"],
                                   (int)jData[i]["stats"]["power"],
                                   (int)jData[i]["stats"]["stamina"],
                                   (int)jData[i]["stats"]["defence"],
                                   (int)jData[i]["stats"]["food"],
                                   jData[i]["description"].ToString(),
                                   jData[i]["i_string"].ToString(),
                                   (bool)jData[i]["stack"]
                                    ));
        }
    }

    public Item GetItem(int id)
    {
        for(int i = 0; i < database.Count; i++)
        {
            if (database[i].id == id)
                return database[i];
        }
        return null;
    }
}

public class Item
{
    public int id { get; set; }
    public string type { get; set; }
    public string title { get; set; }
    public int price { get; set; }
    public int power { get; set; }
    public int stamina { get; set; }
    public int defence { get; set; }
    public int food { get; set; }
    public string description { get; set; }
    public string i_string { get; set; }
    public bool stackable { get; set; }
    public Sprite icon { get; set; }
    public GameObject i_object { get; set; }

    public Item(int id, string type, string title, int price, int power, int stamina, int defence, int food, string description, string i_string, bool stackable)
    {
        this.id = id;
        this.type = type;
        this.title = title;
        this.price = price;
        this.power = power;
        this.stamina = stamina;
        this.defence = defence;
        this.food = food;
        this.description = description;
        this.i_string = i_string;
        this.stackable = stackable;
        this.icon = Resources.Load<Sprite>("Items Icons/" + i_string);
    }
}
