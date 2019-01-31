using Devdog.InventoryPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatReader : MonoBehaviour {

    public InventoryPlayer myPlayer; // Reference to the characterUI we wish to get stats from - Assign in the inspector.

    protected void Start()
    {
        var myStat = myPlayer.stats.Get("categoryName", "statName");
        if (myStat == null)
        {
            Debug.LogWarning("No such stat exists");
            return;
        }
        Debug.Log(myStat.currentValue); // final value can be used for calculations
        Debug.Log(myStat.ToString()); // Formatted name of stat

        // Changing stats
        myStat.ChangeCurrentValueRaw(10f); // Add +10 to our stat.
        myStat.ChangeFactor(-0.1f); // Remove 10% of our stat.
        myStat.SetMaxValueRaw(200f, false); // Set the max value (this is the raw value, so max health can still be increased by the factorMax).

        // And read our value
        Debug.Log("Value after transmutations: " + myStat.currentValue);
    }

}
