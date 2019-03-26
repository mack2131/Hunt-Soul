using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Inventory : MonoBehaviour {

    [Header("Item Prefab")]
    public GameObject inventoryItemPrefab;
    [Header("Main parent inventory panel")]
    public GameObject inventoryPanel;
    [Header("Contetn objects of scroll view")]
    public GameObject gardenContent;
    public GameObject weaponContent;
    [Header("Scroll Views")]
    public GameObject gardenView;
    public GameObject weaponView;
    [Header("Buttons")]
    public Button seedButton;
    public Button weaponButton;

    private List<GameObject> allItems = new List<GameObject>();
    private bool inventoryOpen;

	// Use this for initialization
	void Start ()
    {
        seedButton.onClick.AddListener(SeedOn);
        weaponButton.onClick.AddListener(WeaponOn);
        LoadInv();
        inventoryPanel.transform.localScale = Vector3.zero;
        gardenView.transform.localScale = Vector3.zero;

        inventoryOpen = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*if (Input.GetKey(KeyCode.K))
            PlayerAddItem(0);
        if (Input.GetKeyUp(KeyCode.L))
            SaveInv();*/
        if (Input.GetKeyUp(KeyCode.I) && !inventoryOpen)
        {
            inventoryPanel.transform.localScale = Vector3.one;
            inventoryOpen = true;
        }
        else if (Input.GetKeyUp(KeyCode.I) && inventoryOpen)
        {
            inventoryPanel.transform.localScale = Vector3.zero;
            inventoryOpen = false;
        }
    }

    void LoadInv()
    {
        string[] inv;
        inv = File.ReadAllLines(Application.dataPath + "//StreamingAssets/inv.whatareyoulookingfor");
        for(int i = 0; i < inv.Length; i++)
        {
            int id = int.Parse(inv[i]);
            Item newItem = GameObject.FindObjectOfType<ItemDatabase>().GetItem(id);

            if (newItem.stackable && IsInInv(newItem.id))
                GetItemInInv(newItem.id).GetComponent<InventoryItem>().stack++;
            else switch (newItem.type)
                {
                    case "seed":
                        {
                            GameObject item = GameObject.Instantiate(inventoryItemPrefab, gardenContent.transform, false);
                            item.name = newItem.title;
                            item.GetComponent<InventoryItem>().item = newItem;
                            item.GetComponent<InventoryItem>().descriptionText = inventoryPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                            if (newItem.stackable)
                                item.GetComponent<InventoryItem>().stack = 1;
                            allItems.Add(item);
                            break;
                        }
                }
        }
    }

    public void PlayerAddItem(int id)
    {
        Item newItem = GameObject.FindObjectOfType<ItemDatabase>().GetItem(id);

        if (newItem.stackable && IsInInv(newItem.id))
            GetItemInInv(newItem.id).GetComponent<InventoryItem>().stack++;
        else switch (newItem.type)
            {
                case "seed":
                    {
                        GameObject item = GameObject.Instantiate(inventoryItemPrefab, gardenContent.transform, false);
                        item.name = newItem.title;
                        item.GetComponent<InventoryItem>().item = newItem;
                        item.GetComponent<InventoryItem>().descriptionText = inventoryPanel.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
                        if (newItem.stackable)
                            item.GetComponent<InventoryItem>().stack = 1;
                        allItems.Add(item);
                        break;
                    }
            }
    }

    public void SaveInv()
    {
        int all = 0;
        List<int> inv = new List<int>();
        for (int i = 0; i < allItems.Count; i++)
        {
            if (allItems[i].GetComponent<InventoryItem>().stack > 1)
            {
                int count = allItems[i].GetComponent<InventoryItem>().stack;
                for (int j = 0; j < count; j++)
                {
                    inv.Add(allItems[i].GetComponent<InventoryItem>().item.id);
                    all++;
                }
            }
            else
            {
                inv.Add(allItems[i].GetComponent<InventoryItem>().item.id);
                all++;
            }
        }

        string[] s_inv = new string[all];
        for(int i = 0; i < inv.Count; i++)
        {
            s_inv[i] = inv[i].ToString();
        }
        File.WriteAllLines(Application.dataPath + "//StreamingAssets/inv.whatareyoulookingfor", s_inv);
    }

    public bool IsInInv(int id)
    {
        for(int i = 0; i < allItems.Count; i++)
        {
            if (allItems[i].GetComponent<InventoryItem>().item.id == id)
                return true;
        }
        return false;
    }

    public GameObject GetItemInInv(int id)
    {
        if (IsInInv(id))
        {
            for (int i = 0; i < allItems.Count; i++)
            {
                if (allItems[i].GetComponent<InventoryItem>().item.id == id)
                    return allItems[i];
            }
            return null;
        }
        else return null;
    }

    void SeedOn()
    {
        gardenView.transform.localScale = Vector3.one;
        weaponView.transform.localScale = Vector3.zero;
    }

    void WeaponOn()
    {
        weaponView.transform.localScale = Vector3.one;
        gardenView.transform.localScale = Vector3.zero;
    }
}
