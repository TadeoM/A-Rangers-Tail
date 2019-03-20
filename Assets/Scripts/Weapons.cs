using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : Items {
    public int damage;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int ItemDamage { get { return damage; } set { damage = value; } }
}
