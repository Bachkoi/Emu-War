using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    #region Fields
    public Transform player;
    public Vector3 offset;
    public float followSpeed;
    #endregion

    #region Methods
    /// <summary>
    /// Called once per frame; updates the Camera object.
    /// </summary>
    void Update()
    {
        // Get mouse position
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check if it's inside the player
        bool mouseInsidePlayer = player.GetComponent<Collider2D>().bounds.Contains(mousePos);
        
        // If the mouse is not inside the player, move the camera
        if (!mouseInsidePlayer)
        {
            // Move the camera to the Player's position
            transform.position = Vector3.Lerp(transform.position, player.position + offset, followSpeed);
        }
    }
    #endregion
}
