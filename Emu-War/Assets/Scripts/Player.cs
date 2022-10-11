using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    #region Fields
    public int health;
    public int wheat;
    public int hoardSize;
    public float speed;
    private Animator anim;
    [SerializeReference] private TextMeshProUGUI healthText;
    [SerializeReference] private TextMeshProUGUI wheatText;
    [SerializeReference] private TextMeshProUGUI hoardSizeText;
    #endregion

    #region Methods
    /// <summary>
    /// Initialize Player Fields.
    /// </summary>
    void Start()
    {
        health = 100;
        wheat = 0;
        hoardSize = 0;
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Called once per frame; updates the Player object.
    /// </summary>
    void Update()
    {
        // Movement
        FollowMouse();

        healthText.text = $"Health: {health}";
        wheatText.text = $"Wheat: {wheat}";
        hoardSizeText.text = $"Hoard Size: {hoardSize}";
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
