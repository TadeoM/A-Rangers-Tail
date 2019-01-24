using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Items : MonoBehaviour {
    public string itemName;
    public int itemCost;
    public int durability;
    public int damage;
    public string type;
    public GameObject model;
    public bool stackable;
	// Use this for initialization
	void Start () {
		
	}
    public int ItemDurability { get { return durability; } set { durability = value; } }
    public int ItemDamage { get { return damage; } set { damage = value; } }
    public string ItemName { get { return itemName; } set { itemName = value; } }
    public int ItemCost { get { return itemCost; } set { itemCost = value; } }

    public string ItemType { get { return type; } set { type = value; } }
}
