using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshToggler : MonoBehaviour
{
    public GameObject BatteryPackObject; // This is the GameObject you'll assign in the Inspector
    private bool isActive = true;        // Tracks the current active state

    public void Toggle() // This method will be called by your UI Button
    {
        if (isActive)
        {
            BatteryPackObject.SetActive(false); // Deactivates the GameObject
            isActive = false;                   // Update state
        }
        else
        {
            BatteryPackObject.SetActive(true);  // Activates the GameObject
            isActive = true;                    // Update state
        }
    }
}