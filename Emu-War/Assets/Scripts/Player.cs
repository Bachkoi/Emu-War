using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Fields
    public int health;
    public int wheat;
    public int hordeSize;
    public float speed;
    public List<GameObject> potentialEmus;
    public Queue<GameObject> hordeQueue;
    public List<GameObject> horde;
    public bool isDead;
    public float followRadius = 1.0f;
    private float _speedBuffCooldown;
    private float _speedBuffTimer;
    private bool _speedBuffReady;
    private bool _speedBuffActive;
    private float _wheatSenseCooldown;
    private float _wheatSenseTimer;
    private bool _wheatSenseReady;
    private bool _wheatSenseActive;
    private Animator _anim;
    public float dTime;
    public float score;

    [SerializeReference] private TextMeshProUGUI _healthText;
    [SerializeReference] private TextMeshProUGUI _scoreText;
    [SerializeReference] private TextMeshProUGUI _wheatText;
    [SerializeReference] private TextMeshProUGUI _hordeSizeText;
    [SerializeReference] private Image _speedAbilityProgress;
    [SerializeReference] private Image _wheatAbilityProgress;
    [SerializeReference] private Image _wheatDetector;
    #endregion

    #region Methods
    /// <summary>
    /// Initialize Fields.
    /// </summary>
    void Start()
    {
        hordeQueue = new Queue <GameObject>();
        health = 100;
        wheat = 0;
        hordeSize = 0;
        _anim = GetComponent<Animator>();
        _speedBuffCooldown = 15;
        _speedBuffTimer = 5;
        _speedBuffReady = false;
        _speedBuffActive = false;
        _wheatSenseCooldown = 10;
        _wheatSenseTimer = 5;
        _wheatSenseReady = false;
        _wheatSenseActive = false;
        _wheatDetector.enabled = false;
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
        //FollowMouse();
        Movement();

        if (Input.GetMouseButtonDown(0))
        {
            if (hordeSize > 0)
            {
                HordeShoot();
            }
            else
            {
                // Tell player they don't have a horde to shoot.
            }
        }
        //if (Input.GetKeyDown(KeyCode.Space)){
        //    if(hordeSize > 0)
        //    {
        //        HordeShoot();
        //    }
        //    else
        //    {
        //        // Tell player they don't have a horde to shoot.
        //    }
        //}

        // Update the UI
        _healthText.text = $"Health: {health}";
        _scoreText.text = $"Score: {score}";
        _wheatText.text = $"Wheat: {wheat} / 37";
        _hordeSizeText.text = $"x {hordeSize}";

    }

    private void Movement()
    {
        _anim.SetBool("isWalking", false);
        foreach (GameObject obj in horde)
        {
            obj.GetComponent<Animator>().SetBool("isWalking", false); // Set horde anim to be true.
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            _anim.SetBool("isWalking", true);
            foreach (GameObject obj in horde)
            {
                obj.GetComponent<Animator>().SetBool("isWalking", true); // Set horde anim to be true.
            }

        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            _anim.SetBool("isWalking", true);
            foreach (GameObject obj in horde)
            {
                obj.GetComponent<Animator>().SetBool("isWalking", true); // Set horde anim to be true.
            }
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * speed * Time.deltaTime;
            _anim.SetBool("isWalking", true);
            foreach (GameObject obj in horde)
            {
                obj.GetComponent<Animator>().SetBool("isWalking", true); // Set horde anim to be true.
                obj.GetComponent<Animator>().SetBool("isBackwards", true); // Set horde anim to be true.

            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.up * -speed * Time.deltaTime;
            _anim.SetBool("isWalking", true);
            foreach (GameObject obj in horde)
            {
                obj.GetComponent<Animator>().SetBool("isWalking", true); // Set horde anim to be true.
                obj.GetComponent<Animator>().SetBool("isBackwards", false); // Set horde anim to be true.

            }
        }
    }

    ///<summary>
    /// Collects the Emu's and resultingly changes the speed of wheat collection and radius that the emus will follow in, is called by the horde itself.
    /// </summary>
    public void EmuCollect(GameObject caughtEmu)
    {
        // Check for the collection with the Emus
        horde.Add(caughtEmu);
        hordeQueue.Enqueue(caughtEmu);
        caughtEmu.GetComponent<Horde>().follow = true;
        score += 50.0f;
        followRadius += 0.1f;
        HordeReposition();
    }

    /// <summary>
    /// HordeDeath Method
    /// </summary>
    // This method accepts in the parameter of a GameObject reference of the emu that is shot. It will remove the emu from the collected Horde list.
    // It will then decrement the emuCount field and reduce the following radius. Then go and reposition the overall horde.
    /// <param name="shotEmu"></param>
    public void HordeDeath(GameObject shotEmu)
    {
        shotEmu.gameObject.SetActive(false);
        if (hordeQueue.Count > 0)
        {
            horde.Remove(shotEmu);
            hordeQueue.Dequeue();
            followRadius -= 0.1f;
            HordeReposition();
        }

    }

    ///<summary>
    /// Horde Reposition will take the queue of the horde and then set it to an array to then be able to retrieve the individual game objects.
    /// From there the method will then call the horde classes own reposition method on each horde object and then requeue them and update the necessary fields.
    /// </summary>
    public void HordeReposition()
    {
        GameObject[] tempArr = hordeQueue.ToArray();
        hordeQueue.Clear();
        for (int i = 0; i < tempArr.Length; i++)
        {
            // Attempt with Queue
            tempArr[i].GetComponent<Horde>().FollowRadius = followRadius;
            tempArr[i].GetComponent<Horde>().Reposition(((float)i + 1) * (360.0f / tempArr.Length));
            hordeQueue.Enqueue(tempArr[i]);
        }
        hordeSize = hordeQueue.Count;
    }

    /// <summary>
    /// Horde Shoot will be triggered by pressing a button for now will be the space bar to shoot an emu in the direction and will stop when it collides with a wall.
    /// </summary>
    public void HordeShoot()
    {
        GameObject thrownEmu = hordeQueue.Dequeue();
        horde.Remove(thrownEmu);
        followRadius -= 0.1f;
        thrownEmu.GetComponent<Horde>().isThrown = true;
        thrownEmu.GetComponent<Horde>().follow = false;

        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = 0;

        thrownEmu.transform.position = Vector3.MoveTowards(thrownEmu.transform.position, targetPos, speed * Time.deltaTime);
        thrownEmu.GetComponent<Horde>().throwPos = targetPos;
        HordeReposition();
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

            _speedAbilityProgress.fillAmount = 1 - (_speedBuffCooldown / 15);
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

            _speedAbilityProgress.fillAmount = (_speedBuffTimer / 5);

            // If the timer falls below 0s remaining
            if (_speedBuffTimer <= 0)
            {
                // Reset the speed 
                speed = 5;

                // Reset the timers
                _speedBuffCooldown = 15;
                _speedBuffTimer = 5;

                // Set the buff's active state
                _speedBuffActive = false;

                // Reset progress color
                _speedAbilityProgress.color = Color.white;
            }
        }
        #endregion

        #region WheatSense
        // If the buff is on cooldown
        if (_wheatSenseCooldown > 0)
        {
            // Reduce the time from the cooldown
            _wheatSenseCooldown -= Time.deltaTime;
            _wheatAbilityProgress.fillAmount = 1 - (_wheatSenseCooldown / 10);
        }
        // If the buff is not on cooldown and not in use
        else if (_wheatSenseCooldown <= 0 && !_wheatSenseActive)
        {
            // Set state to ready
            _wheatSenseReady = true;
        }

        // If the buff is active
        if (_wheatSenseActive)
        {
            // Calculate and animate detector
            _wheatDetector.transform.rotation = Quaternion.RotateTowards(_wheatDetector.transform.rotation, Quaternion.Euler(new Vector3(0, 0, FindClosestWheatAngle())), 360 * Time.deltaTime);

            // Reduce active timer
            _wheatSenseTimer -= Time.deltaTime;

            _wheatAbilityProgress.fillAmount = (_wheatSenseTimer / 5);

            // If the timer falls below 0s remaining
            if (_wheatSenseTimer <= 0)
            {
                // Hide the wheat sense
                _wheatDetector.enabled = false;

                // Reset the timers
                _wheatSenseCooldown = 10;
                _wheatSenseTimer = 5;

                // Set the buff's active state
                _wheatSenseActive = false;

                // Reset progress color
                _wheatAbilityProgress.color = Color.white;
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

        // Wheat buff
        if (Input.GetKeyDown(KeyCode.Alpha2) && _wheatSenseReady)
        {
            // Set buff states
            _wheatSenseReady = false;
            _wheatSenseActive = true;

            // Show sense
            _wheatDetector.enabled = true;

            // Change the progress color
            _wheatAbilityProgress.color = Color.yellow;
        }
    }

    /// <summary>
    /// Finds the closest wheat and the angle to it.
    /// </summary>
    /// <returns>The angle to the nearest wheat</returns>
    private float FindClosestWheatAngle()
    {
        // Get all wheat game objects
        GameObject[] _wheatCrops;
        _wheatCrops = GameObject.FindGameObjectsWithTag("Wheat");

        // Find closest wheat
        GameObject _closest = null;
        float _distance = Mathf.Infinity;
        Vector3 _position = transform.position;
        foreach (GameObject _wheat in _wheatCrops)
        {
            Vector3 _diff = _wheat.transform.position - _position;
            float _curDistance = _diff.sqrMagnitude;
            if (_curDistance < _distance)
            {
                _closest = _wheat;
                _distance = _curDistance;
            }
        }

        // If the y position of the wheat is less than the y position of the player, change the sign to a negative
        float _sign = _closest.transform.position.y < _position.y ? -1.0f : 1.0f;

        // Calculate and return the angle
        return Vector2.Angle(Vector2.right, _closest.transform.position - _position) * _sign + 15;
    }


    public float PlayerScore()
    {
        if (score != 0)
        {
            return score;
        }
        if (!isDead)
        {
            switch (dTime)
            {
                case < 120:
                    score += 5000;
                    break;
                case < 180:
                    score += 4000;
                    break;
                case < 240:
                    score += 3000;
                    break;
                case < 300:
                    score += 2000;
                    break;
                case < 360:
                    score += 1000;
                    break;
            }
        }
        //score += (50.0f * hordeSize);
        //score += (100.0f * wheat);

        return score;
    }
    #endregion
}
