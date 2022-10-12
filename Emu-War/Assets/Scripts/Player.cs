using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    #region Fields
    public float speed;
    Animator anim;
    public List<GameObject> horde;
    public float followRadius = 0.5f;
    public int emuCount = 0;
    private LineRenderer _lineRenderer;
    #endregion

    #region Methods
    void Start()
    {
        anim = GetComponent<Animator>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.SetVertexCount(360);
    }

    /// <summary>
    /// Called once per frame; updates the Player object.
    /// </summary>
    void Update()
    {
        // Movement
        FollowMouse();
        DrawCircle();
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
            foreach(GameObject obj in horde)
            {
                obj.GetComponent<Animator>().SetBool("isWalking", true);
            }

            // Transform the Player object toward the target position
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("isWalking", false);
            foreach (GameObject obj in horde)
            {
                obj.GetComponent<Animator>().SetBool("isWalking", false);
            }
        }
    }

    ///<summary>
    /// Collects the Emu's and resultingly changes the speed of wheat collection and radius that the emus will follow in
    /// </summary>
    public void EmuCollect()
    {
        // Check for the collection with the Emus
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Console.WriteLine(collision.gameObject);
        if (collision.tag == "Emu")
        {
            Console.WriteLine("HIT");
            collision.gameObject.GetComponent<Horde>().follow = true;
            horde.Add(collision.gameObject);
            followRadius += 0.1f;
            for(int i = 0; i < horde.Count; i++)
            {
                horde[i].GetComponent<Horde>().FollowRadius = followRadius;
                horde[i].GetComponent<Horde>().Reposition((float)i+1 * (360.0f / horde.Count));
            }
            /*foreach(GameObject obj in horde)
            {
                obj.GetComponent<Horde>().FollowRadius = followRadius;
                obj.GetComponent<Horde>().Reposition(horde.Count);
            }*/
            emuCount = horde.Count;
        }
    }
    
    public void DrawCircle() 
    {
        for(int i = 0; i < 360; i++)
        {
            float x = followRadius * (float)Math.Cos(Mathf.Deg2Rad * (double)i);
            float y = followRadius * (float)Math.Sin(Mathf.Deg2Rad * (double)i);
            _lineRenderer.SetPosition(i, new Vector3(transform.position.x + (float)x, transform.position.y + (float)y, 0));
        }
    }
    #endregion
}
