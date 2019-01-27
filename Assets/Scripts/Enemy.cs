using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Creature_v2 {

    protected int currentSide;
    protected bool foundTarget;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    protected abstract void PerformAttack();
}
