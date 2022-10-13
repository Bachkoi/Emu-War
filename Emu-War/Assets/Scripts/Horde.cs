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
    private int _orientation;
    public Vector2 gap;
    Animator anim;
    #endregion

    #region Properties
    public float FollowRadius
    {
        set { _followRadius = value; }
        get { return _followRadius; }
    }
    public int Orientation
    {
        set { _orientation = value; }
        get { return _orientation; }
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
                Vector2 tempPos = transform.position;
                tempPos.x = player.transform.position.x + gap.x;
                tempPos.y = player.transform.position.y + gap.y;
                transform.position = tempPos;
                /*switch (Orientation)
                {
                    case 3:
                        tempPos.x = player.transform.position.x + _followRadius;
                        tempPos.y = player.transform.position.y - _followRadius;
                        transform.position = tempPos;
                        break;
                    case 2:
                        tempPos.x = player.transform.position.x - _followRadius;
                        tempPos.y = player.transform.position.y - _followRadius;
                        transform.position = tempPos;
                        break;
                    case 1:
                        tempPos.x = player.transform.position.x - _followRadius;
                        tempPos.y = player.transform.position.y + _followRadius;
                        transform.position = tempPos;
                        break;
                    default:
                        tempPos.x = player.transform.position.x + _followRadius;
                        tempPos.y = player.transform.position.y + _followRadius;
                        transform.position = tempPos;
                        break;

                }

                switch (Orientation)
                {
                    case 3:
                        tempPos.x = player.transform.position.x + _followRadius;
                        tempPos.y = player.transform.position.y - _followRadius;
                        transform.position = tempPos;
                        break;
                    case 2:
                        tempPos.x = player.transform.position.x - _followRadius;
                        tempPos.y = player.transform.position.y - _followRadius;
                        transform.position = tempPos;
                        break;
                    case 1:
                        tempPos.x = player.transform.position.x - _followRadius;
                        tempPos.y = player.transform.position.y + _followRadius;
                        transform.position = tempPos;
                        break;
                    default:
                        tempPos.x = player.transform.position.x + _followRadius;
                        tempPos.y = player.transform.position.y + _followRadius;
                        transform.position = tempPos;
                        break;

                }*/
                /*System.Random rng = new System.Random();
                double rngValue = rng.NextDouble();
                Vector2 tempPos = transform.position;
                tempPos.x = player.transform.position.x + _followRadius;
                tempPos.y = player.transform.position.y - _followRadius;
                transform.position = tempPos;*/

                /*if (rngValue <= 0.25) // Bottom Right
                {
                    tempPos.x = player.transform.position.x + _followRadius;
                    tempPos.y = player.transform.position.y - _followRadius;
                    transform.position = tempPos;
                }
                else if (rngValue >= 0.25 && rngValue <= 0.5) // Bottom Left
                {
                    tempPos.x = player.transform.position.x - _followRadius;
                    tempPos.y = player.transform.position.y - _followRadius;
                    transform.position = tempPos;
                }
                else if (rngValue >= 0.5 && rngValue <= 0.75) // Top left
                {
                    tempPos.x = player.transform.position.x - _followRadius;
                    tempPos.y = player.transform.position.y + _followRadius;
                    transform.position = tempPos;
                }
                else // Top right 
                {
                    tempPos.x = player.transform.position.x + _followRadius;
                    tempPos.y = player.transform.position.y + _followRadius;
                    transform.position = tempPos;
                }*/
            }

        }
    }
    /// <summary>
    /// Will reposition and move the emus around whenever a new one is connected or lost
    /// </summary>
    public void Reposition(float theta)
    {
        System.Random rng = new System.Random();
        //float rotation = 360.0f / hordeCount;
        float rotationX = rng.Next(360);
        float rotationY = rng.Next(360);
        double rngValue = rng.NextDouble();
        double percentage = rng.NextDouble();
        Vector2 tempPos = transform.position;
        //gap.x = _followRadius * (float)Math.Sin(rotationX);
        //gap.y = _followRadius * (float)Math.Cos(rotationY);
        gap.x = (float)(rng.Next(40,100)/100.0f) * (_followRadius * (float)Math.Cos(theta));
        gap.y = (float)(rng.Next(40,100)/100.0f) * (_followRadius * (float)Math.Sin(theta));
        tempPos.x = player.transform.position.x + gap.x;
        tempPos.y = player.transform.position.y + gap.y;
        transform.position = tempPos;
        //tempPos.x = player.transform.position.x + _followRadius;
        //tempPos.y = player.transform.position.y - _followRadius;
        //transform.position = tempPos;
        if (rngValue <= 0.25) // Bottom Right
        {
            //tempPos.x = player.transform.position.x + _followRadius;
            //tempPos.y = player.transform.position.y - _followRadius;
            //transform.position = tempPos;
            Orientation = 0;
        }
        else if (rngValue >= 0.25 && rngValue <= 0.5) // Bottom Left
        {
            //tempPos.x = player.transform.position.x - _followRadius;
            //tempPos.y = player.transform.position.y - _followRadius;
            //transform.position = tempPos;
            Orientation = 1;
        }
        else if (rngValue >= 0.5 && rngValue <= 0.75) // Top left
        {
            //tempPos.x = player.transform.position.x - _followRadius;
            //tempPos.y = player.transform.position.y + _followRadius;
            //transform.position = tempPos;
            Orientation = 2;
        }
        else // Top right 
        {
            /*tempPos.x = player.transform.position.x + _followRadius;
            tempPos.y = player.transform.position.y + _followRadius;
            transform.position = tempPos;*/
            Orientation = 3;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Emu")
        {
            Console.WriteLine("HIT EE");
            if (collision.gameObject.GetComponent<Horde>().follow == false)
            {
                //player.EmuCollect(collision);
                collision.gameObject.GetComponent<Horde>().follow = true;
                player.horde.Add(collision.gameObject);
                player.followRadius += 0.1f;
                for (int i = 0; i < player.horde.Count; i++)
                {
                    player.horde[i].GetComponent<Horde>().FollowRadius = player.followRadius;
                    player.horde[i].GetComponent<Horde>().Reposition((float)i + 1 * (360.0f / player.horde.Count));
                }
                /*foreach(GameObject obj in horde)
                {
                    obj.GetComponent<Horde>().FollowRadius = followRadius;
                    obj.GetComponent<Horde>().Reposition(horde.Count);
                }*/
                player.emuCount = player.horde.Count;
            }
            /*
            collision.gameObject.GetComponent<Horde>().follow = true;
            player.horde.Add(collision.gameObject);
            player.followRadius += 0.1f;
            for (int i = 0; i < player.horde.Count; i++)
            {
                player.horde[i].GetComponent<Horde>().FollowRadius = player.followRadius;
                player.horde[i].GetComponent<Horde>().Reposition((float)i + 1 * (360.0f / player.horde.Count));
            }
            /*foreach(GameObject obj in horde)
            {
                obj.GetComponent<Horde>().FollowRadius = followRadius;
                obj.GetComponent<Horde>().Reposition(horde.Count);
            }*/
            //player.emuCount = player.horde.Count;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        //Console.WriteLine(collision.gameObject);
        if (collision.tag == "Emu")
        {
            Console.WriteLine("HIT EE");
            if(collision.gameObject.GetComponent<Horde>().follow == false)
            {
                //player.EmuCollect(collision);
            }
            /*
            collision.gameObject.GetComponent<Horde>().follow = true;
            player.horde.Add(collision.gameObject);
            player.followRadius += 0.1f;
            for (int i = 0; i < player.horde.Count; i++)
            {
                player.horde[i].GetComponent<Horde>().FollowRadius = player.followRadius;
                player.horde[i].GetComponent<Horde>().Reposition((float)i + 1 * (360.0f / player.horde.Count));
            }
            /*foreach(GameObject obj in horde)
            {
                obj.GetComponent<Horde>().FollowRadius = followRadius;
                obj.GetComponent<Horde>().Reposition(horde.Count);
            }*/
            //player.emuCount = player.horde.Count;
        }
    }


}
