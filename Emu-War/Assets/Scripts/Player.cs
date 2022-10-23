using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Fields
    public int health;
    public int wheat;
    public int hordeSize;
    public float speed;
    private float _speedBuffCooldown;
    private float _speedBuffTimer;
    private bool _speedBuffReady;
    private bool _speedBuffActive;
    private Animator _anim;
    [SerializeReference] private TextMeshProUGUI _healthText;
    [SerializeReference] private TextMeshProUGUI _wheatText;
    [SerializeReference] private TextMeshProUGUI _hordeSizeText;
    [SerializeReference] private Image _speedAbilityProgress;
    #endregion

    #region Methods
    /// <summary>
    /// Initialize Fields.
    /// </summary>
    void Start()
    {
        health = 100;
        wheat = 0;
        hordeSize = 0;
        _anim = GetComponent<Animator>();
        _speedBuffCooldown = 5;
        _speedBuffTimer = 5;
        _speedBuffReady = false;
        _speedBuffActive = false;
    }

    /// <summary>
    /// Called once per frame; updates the Player object.
    /// </summary>
    void Update()
    {
        // Update ability timers
        UpdateAbilityTimers();

        // Use ability
        UseAbility();

        // Movement
        FollowMouse();

        // Update the UI
        _healthText.text = $"Health: {health}";
        _wheatText.text = $"Wheat: {wheat} / 33";
        _hordeSizeText.text = $"Horde Size: {hordeSize}";
        _speedAbilityProgress.fillAmount = 1 - (_speedBuffCooldown / 5);
    }

    /// <summary>
    /// Enables the Player object to move by following the mouse.
    /// </summary>
    private void FollowMouse()
    {
        // Get the target position
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = -1;

        // Check if the mouse is inside the player
        bool mouseInsidePlayer = GetComponent<Collider2D>().bounds.Contains(targetPos);

        // If it isn't, move the player
        if (!mouseInsidePlayer)
        {
            _anim.SetBool("isWalking", true);

            // Transform the Player object toward the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        else
        {
            _anim.SetBool("isWalking", false);
        }
    }

    /// <summary>
    /// Updates all ability timers
    /// </summary>
    private void UpdateAbilityTimers()
    {
        #region SpeedBuff
        // If the buff is on cooldown
        if (_speedBuffCooldown > 0)
        {
            // Reduce the time from the cooldown
            _speedBuffCooldown -= Time.deltaTime;
        }
        // If the buff is not on cooldown and not in use
        else if (_speedBuffCooldown <= 0 && !_speedBuffActive)
        {
            // Set state to ready
            _speedBuffReady = true;
        }

        // If the buff is active
        if (_speedBuffActive)
        {
            // Reduce active timer
            _speedBuffTimer -= Time.deltaTime;

            // If the timer falls below 0s remaining
            if (_speedBuffTimer <= 0)
            {
                // Reset the speed 
                speed = 5;

                // Reset the timers
                _speedBuffCooldown = 5;
                _speedBuffTimer = 5;

                // Set the buff's active state
                _speedBuffActive = false;

                // Reset progress color
                _speedAbilityProgress.color = Color.white;
            }
        }
        #endregion
    }

    /// <summary>
    /// Method that handles various ability use
    /// </summary>
    private void UseAbility()
    {
        // Speed buff
        if (Input.GetKeyDown(KeyCode.Alpha1) && _speedBuffReady)
        {
            // Set the speed
            speed = 10;

            // Set buff states
            _speedBuffReady = false;
            _speedBuffActive = true;

            // Change the progress color
            _speedAbilityProgress.color = Color.yellow;
        }
    }
    #endregion
}
