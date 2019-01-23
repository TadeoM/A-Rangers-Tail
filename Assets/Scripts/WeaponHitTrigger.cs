using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitTrigger : Weapon
{
    public float dmg;

    private void Awake()
    {
        dmg = WeaponDmg;
    }
    // Use this for initialization
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.isTrigger!=true && col.CompareTag("Enemy"))
        {
            col.SendMessageUpwards("Damage", dmg);
        }
    }
}
