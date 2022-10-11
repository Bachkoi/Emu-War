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
    private Animator _anim;
    [SerializeReference] private TextMeshProUGUI _healthText;
    [SerializeReference] private TextMeshProUGUI _wheatText;
    [SerializeReference] private TextMeshProUGUI _hoardSizeText;
    #endregion

    #region Methods
    /// <summary>
    /// Initialize Fields.
    /// </summary>
    void Start()
    {
        health = 100;
        wheat = 0;
        hoardSize = 0;
        _anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Called once per frame; updates the Player object.
    /// </summary>
    void Update()
    {
        // Movement
        FollowMouse();

        // Update the UI
        _healthText.text = $"Health: {health}";
        _wheatText.text = $"Wheat: {wheat}";
        _hoardSizeText.text = $"Hoard Size: {hoardSize}";
    }

    /// <summary>
    /// Enables the Player object to move by following the mouse.
    /// </summary>
    private void FollowMouse()
    {
        // Get the target position
        Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check if the mouse is inside the player
        bool mouseInsidePlayer = GetComponent<Collider2D>().bounds.Contains(targetPos);

        // If it isn't, move the player
        if (!mouseInsidePlayer)
        {
            _anim.SetBool("isWalking", true);

            // Transform the Player object toward the target position
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        else
        {
            _anim.SetBool("isWalking", false);
        }
    }
    #endregion
}
