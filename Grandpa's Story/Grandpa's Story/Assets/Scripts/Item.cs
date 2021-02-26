using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class Item
{
   
    public string itemName;
    public Sprite sprite;
    public string displayName;

    public Item(string itemName, Sprite sprite, string displayName)
    {
        this.itemName = itemName;
        this.sprite = sprite;
        this.displayName = displayName;
    }
}
