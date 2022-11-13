using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheat : MonoBehaviour
{
    #region Fields
    public float health;
    public Player player;
    #endregion

    #region Methods
    /// <summary>
    /// Initialize Wheat Fields.
    /// </summary>
    void Start()
    {
        // Set initial health value
        health = 100;
    }

    private void Update()
    {
        // If there's no remaining health
        if (health <= 0)
        {
            // Count collected wheat
            player.wheat++;
            player.score += 100.0f;

            // Destroy the wheat
            Destroy(gameObject);
        }

    }

    /// <summary>
    /// Checks Triggers when GameObjects collide with Wheat.
    /// </summary>
    /// <param name="collision">The collision occurring</param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Emu")){
            // Reduce wheat health based on number of Emus
            health -= 1 + (player.hordeSize * 0.025f);
        }
        else if (collision.gameObject.CompareTag("Horde")){
            health -= 0.1f + (player.hordeSize * 0.025f);
        }
    }
    #endregion
}
