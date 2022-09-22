using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Fields
    public float speed;
    Animator anim;
    #endregion

    #region Methods
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Called once per frame; updates the Player object.
    /// </summary>
    void Update()
    {
        // Movement
        FollowMouse();
    }

    /// <summary>
    /// Enables the Player object to move by following the mouse.
    /// </summary>
    private void FollowMouse()
    {
        // Get the target position
        Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get the object's current position
        Vector2 objectPos = Camera.main.WorldToScreenPoint(transform.position);

        // Check if the mouse is inside the player
        bool mouseInsidePlayer = GetComponent<Collider2D>().bounds.Contains(targetPos);

        // If it isn't, move the player
        if (!mouseInsidePlayer)
        {
            anim.SetBool("isWalking", true);

            // Transform the Player object toward the target position
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }
    #endregion
}
