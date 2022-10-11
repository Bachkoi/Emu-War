using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatHealth : MonoBehaviour
{
    #region Fields
    private Vector3 localScale;
    private GameObject wheat;
    #endregion

    #region Methods
    /// <summary>
    /// Initialize the Wheat Health Bar.
    /// </summary>
    void Start()
    {
        // Initialize the scale of the health bar
        localScale = transform.localScale;

        // Get the parent Wheat object
        wheat = gameObject.transform.parent.gameObject;
    }

    /// <summary>
    /// Update the health bar.
    /// </summary>
    void Update()
    {
        // Get the health of the Wheat crop
        float drawHealth = wheat.GetComponent<Wheat>().health;
        
        // If it has been partially eaten
        if (drawHealth < 100)
        {
            // Show the health bar
            localScale.x = drawHealth / 100 * 1.5f;
        }
        else
        {
            // Hide the health bar
            localScale.x = 0;
        }
        
        // Set the scale
        transform.localScale = localScale;
    }
    #endregion
}
