using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitTrigger : MonoBehaviour
{ 
    int dmg;
    PlayerAttack playerAttk;
    GameObject hittarget;

    private void Awake()
    {
        dmg = gameObject.GetComponentInParent<PlayerAttack>().dmg;
    }
    // Use this for initialization
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("bug"))
        {
           
        }
    }
}
