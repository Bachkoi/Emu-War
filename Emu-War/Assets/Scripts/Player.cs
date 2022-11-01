using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

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
    public float followRadius = 1.0f;
    private Animator _anim;
    [SerializeReference] private TextMeshProUGUI _healthText;
    [SerializeReference] private TextMeshProUGUI _wheatText;
    [SerializeReference] private TextMeshProUGUI _hordeSizeText;
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
    }

    /// <summary>
    /// Called once per frame; updates the Player object.
    /// </summary>
    void Update()
    {
        // Movement
        FollowMouse();
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
        _wheatText.text = $"Wheat: {wheat} / 33";
        _hordeSizeText.text = $"Horde Size: {hordeSize}";
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
            foreach(GameObject obj in horde)
            {
                obj.GetComponent<Animator>().SetBool("isWalking", true); // Set horde anim to be true.
            }
            _anim.SetBool("isWalking", true);

            // Transform the Player object toward the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        else
        {
            foreach (GameObject obj in horde)
            {
                obj.GetComponent<Animator>().SetBool("isWalking", false); // Set the horde anim to be false. COMMENTED UNTIL WE HAVE THE ANIMATIONS WORKING
            }
            _anim.SetBool("isWalking", false);
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
    #endregion
}
