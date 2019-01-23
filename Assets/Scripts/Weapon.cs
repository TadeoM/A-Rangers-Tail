using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base Weapon Class
public class Weapon : MonoBehaviour
{

    public int durability;
    public int cost;
    public string wpName;
    public string type;
    public float damage;

    // Use this for initialization
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public float WeaponDmg
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }

    public int WeaponDurability
    { get
        {
            return durability;
        }
        set
        {
            durability = value;
        }
    }
    public int WeaponCost
    {
        get
        {
            return cost;
        }
        set
        {
            cost = value;
        }
    }
    public string WeaponName
    {
        get
        {
            return wpName;
        }
        set
        {
            wpName = value;
        }
    }
}
