using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    #region Fields
    public float speed;
    Animator anim;
    public List<GameObject> potentialEmus;
    public Queue<GameObject> hordeQueue;
    public List<GameObject> horde;
    public float followRadius = 1.0f;
    public int emuCount = 0;
    #endregion

    #region Methods
    void Start()
    {
        anim = GetComponent<Animator>();
        hordeQueue = new Queue <GameObject>();
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

        // FOR Sprint 4, have the horde utilize the FollowMouse method to make it more fluid.

        // If it isn't, move the player
        if (!mouseInsidePlayer)
        {
            anim.SetBool("isWalking", true);
            foreach(GameObject obj in horde)
            {
                obj.GetComponent<Animator>().SetBool("isWalking", true); // Set horde anim to be true.
            }

            // Transform the Player object toward the target position
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("isWalking", false);
            foreach (GameObject obj in horde)
            {
                obj.GetComponent<Animator>().SetBool("isWalking", false); // Set the horde anim to be false.
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
        followRadius += 0.1f;
        HordeReposition();
    }
    /// <summary>
    /// HordeDeath Method
    /// </summary>
    // This method accepts in the parameter of a GameObject reference of the emu that is shot. It will remove the emu from the collected Horde list.
    // It will then decrement the emuCount field and reduce the following radius. Then go and reposition the overall horde.
    /// <param name="shotEmu"></param>
    public void HordeDeath(GameObject shotEmu){
        
        horde.Remove(shotEmu);
        hordeQueue.Dequeue();

        followRadius -= 0.1f;
        shotEmu.gameObject.SetActive(false);

        HordeReposition();
    }

    ///<summary>
    /// 
    /// </summary>
    public void HordeReposition()
    {
        GameObject[] tempArr = hordeQueue.ToArray();
        hordeQueue.Clear();
        for (int i = 0; i < tempArr.Length; i++)
        {
            //horde[i].GetComponent<Horde>().FollowRadius = followRadius;
            //horde[i].GetComponent<Horde>().Reposition((float)i + 1 * (360.0f / horde.Count));

            // Attempt with Queue
            tempArr[i].GetComponent<Horde>().FollowRadius = followRadius;
            tempArr[i].GetComponent<Horde>().Reposition(((float)i + 1) * (360.0f / tempArr.Length));
            hordeQueue.Enqueue(tempArr[i]);
        }
        emuCount = hordeQueue.Count;
    }
    #endregion
}
