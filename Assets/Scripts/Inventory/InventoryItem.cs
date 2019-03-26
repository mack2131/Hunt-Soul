using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventoryItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public Item item;
    public int id;
    public Text titleText;
    public Text stackText;
    public Image icon;
    public Text statsText;
    public Text descriptionText;
    public bool stackable;
    public int stack;

	// Use this for initialization
	void Start ()
    {
        InitItem();
	}
	
	// Update is called once per frame
	/*void Update ()
    {
        if (item.stackable)
            stackText.text = stack.ToString();
	}*/

    void InitItem()
    {
        id = item.id;
        titleText.text = item.title;
        icon.sprite = item.icon;
        stackable = item.stackable;
        statsText.text = item.power + "\n" + item.stamina + "\n" + item.defence + "\n" + item.food;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionText.text = item.description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionText.text = "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }
}
