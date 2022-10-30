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
    public bool isThrown = false;
    public Vector2 gap;
    public Vector2 wallPos;
    public Vector3 throwPos;
    public int hordeSize = 0;
    //Animator anim;
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
        //anim = GetComponent<Animator>();
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
            }
            if (isDead == true)
            {
                player.HordeDeath(this.gameObject);
            }
            if(isThrown == true)
            {
                this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, throwPos, _speed * Time.deltaTime);
                wallPos = this.gameObject.transform.position;
                if(this.gameObject.transform.position == throwPos)
                {
                    isThrown = false;
                }
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

        tempPos.x = player.transform.position.x + gap.x;
        tempPos.y = player.transform.position.y + gap.y;
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
                if (follow == false && isThrown == false)
                {
                    player.EmuCollect(this.gameObject);
                }
                break;
            case "Horde":
                if(follow == false && isThrown == false)
                {
                    if(collision.gameObject.GetComponent<Horde>().isThrown == false)
                    {
                        player.EmuCollect(this.gameObject);
                    }
                }
                break;

            case "bullet":
                if(follow == true){
                    player.HordeDeath(this.gameObject);
                    isDead = true;
                }
                break;

            case "Obstacle":
                if (follow == true)
                {
                    this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, player.transform.position, _speed * Time.deltaTime);
                    inWall = true;
                    // NEED TO CALL THIS IN UPDATE TOO
                }
                else
                {
                    throwPos = collision.transform.localPosition;
                    this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, wallPos, _speed * Time.deltaTime);
                    //throwPos = this.gameObject.transform.position;
                    //_speed = 0f;
                    isThrown = false;
                }
                break;

            case "Wall":
                if (follow == true)
                {
                    this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, player.transform.position, _speed * Time.deltaTime);
                    inWall = true;
                    // NEED TO CALL THIS IN UPDATE TOO
                }
                else
                {
                    //throwPos = collision.transform.localPosition;
                    this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, wallPos, _speed * Time.deltaTime);
                   //throwPos = collision.transform.localPosition;
                    //_speed = 0f;
                    isThrown = false;
                }
                break;
        }
        
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Obstacle" || collision.tag == "Wall")
        {
            inWall = false;
            if(isThrown == true)
            {
                isThrown = false;
                follow = false;
            }
        }
    }
}
