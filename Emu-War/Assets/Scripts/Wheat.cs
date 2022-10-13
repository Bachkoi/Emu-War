using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheat : MonoBehaviour
{
    #region Fields
    public float health;
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

    /// <summary>
    /// Checks Triggers when GameObjects collide with Wheat.
    /// </summary>
    /// <param name="collision">The collision occurring</param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        // If the Wheat collides with an emu...
        if (collision.gameObject.CompareTag("Emu"))
        {
            // Reduce wheat health based on number of Emus
            health -= 1 + (collision.gameObject.GetComponent<Player>().hordeSize * 0.1f);

            // If there's no remaining health
            if (health <= 0)
            {
                // Count collected wheat
                collision.gameObject.GetComponent<Player>().wheat++;

                // Destroy the wheat
                Destroy(gameObject);
            }
        }
    }
    #endregion
}
