using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Horde : MonoBehaviour
{
    #region Fields
    public Player player;
    private float _followRadius = 1.0f;
    public bool follow = false;
    public bool inWall = false;
    public Vector2 gap;
    public Vector2 wallPos;
    Animator anim;
    public bool isDead;
    private float _speed = 5f;
    #endregion

    #region Properties
    public float FollowRadius
    {
        set { _followRadius = value; }
        get { return _followRadius; }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) // Check to make sure the player is alive
        {
            if(follow == true) // Check to make sure the Emu is collected
            {
                Vector2 tempPos = this.gameObject.transform.position;
                tempPos.x = player.transform.position.x + gap.x;
                tempPos.y = player.transform.position.y + gap.y;
                if (inWall)
                {
                    this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, player.transform.position, _speed * Time.deltaTime);
                }
                else
                {
                    this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, tempPos, _speed * Time.deltaTime);
                }
                //transform.position = tempPos;
            }
            if (isDead == true)
            {
                player.HordeDeath(this.gameObject);
            }
        }
    }
    /// <summary>
    /// Will reposition and move the emus around whenever a new one is connected or lost
    /// Utilizies RNG to generate how far away from the Emu in the poisiion. Emus should be evenly
    /// Distributed throughout the circle.
    /// </summary>
    public void Reposition(float theta)
    {
        System.Random rng = new System.Random();
        Vector2 tempPos = this.gameObject.transform.position;

        gap.x = (float)((rng.Next(40, 100)) / 100.0f) * (_followRadius * MathF.Cos(theta));
        gap.y = (float)((rng.Next(40, 100)) / 100.0f) * (_followRadius * MathF.Sin(theta));

        //gap.x = (float)((rng.Next(40,100)) / 100.0f) * (_followRadius * (float)Math.Cos(theta));
        //gap.y = (float)((rng.Next(40,100)) / 100.0f) * (_followRadius * (float)Math.Sin(theta));

        tempPos.x = player.transform.position.x + gap.x;
        tempPos.y = player.transform.position.y + gap.y;
        //transform.position = tempPos;
        this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, tempPos, _speed * Time.deltaTime);
    }

    /// <summary>
    /// OnTriggerEnter2D is utitlized to check its collision with either bullets or emus or wheat.
    /// </summary>
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Console.WriteLine(collision.gameObject);
        switch(collision.tag){
            case "Emu":
                if(follow == false){
                    player.EmuCollect(this.gameObject);
                }
                break;

            case "bullet":
                if(follow == true){
                    if(isDead == true){
                        player.HordeDeath(this.gameObject);
                    }
                }
                break;

            case "Obstacle":
                if(follow == true)
                {
                    this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, player.transform.position, _speed * Time.deltaTime);
                    inWall = true;
                    // NEED TO CALL THIS IN UPDATE TOO
                }
                break;
        }
        
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Obstacle")
        {
            inWall = false;
        }
    }
}
