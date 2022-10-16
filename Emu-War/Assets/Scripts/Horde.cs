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
    public Vector2 gap;
    Animator anim;
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
                this.gameObject.transform.position = Vector2.MoveTowards(this.gameObject.transform.position, tempPos, _speed * Time.deltaTime);
                //transform.position = tempPos;
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

        gap.x = (float)((rng.Next(40, 100)) / 100.0f) * (_followRadius * MathF.Cos(2));
        gap.y = (float)((rng.Next(40, 100)) / 100.0f) * (_followRadius);

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
                    player.HordeDeath(this.gameObject);
                }
                break;
        }
        
    }
}
