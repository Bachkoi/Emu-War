using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatHealth : MonoBehaviour
{
    #region Fields
    private Vector3 _localScale;
    private GameObject _wheat;
    #endregion

    #region Methods
    /// <summary>
    /// Initialize the Wheat Health Bar Fields.
    /// </summary>
    void Start()
    {
        // Initialize the scale of the health bar
        _localScale = transform.localScale;

        // Get the parent Wheat object
        _wheat = gameObject.transform.parent.gameObject;
    }

    /// <summary>
    /// Called once per frame; updates the Wheat Health Bar object.
    /// </summary>
    void Update()
    {
        // Get the health of the Wheat crop
        float _drawHealth = _wheat.GetComponent<Wheat>().health;
        
        // If it has been partially eaten
        if (_drawHealth < 100)
        {
            // Show the health bar
            _localScale.x = _drawHealth / 100 * 1.5f;
        }
        else
        {
            // Hide the health bar
            _localScale.x = 0;
        }
        
        // Set the scale
        transform.localScale = _localScale;
    }
    #endregion
}
