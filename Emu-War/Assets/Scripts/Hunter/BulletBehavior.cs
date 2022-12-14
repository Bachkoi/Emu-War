using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private float _bulletSpeed;
    private int _lifetimeCount;
    public Player player;
    #endregion Fields

    private void Start()
    {
        _lifetimeCount = 0;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * _bulletSpeed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, -1.0f);
        _lifetimeCount++;
        if(_lifetimeCount > 30000)
        {
            _lifetimeCount = 0;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the bullet collides with an emu...
        if (collision.gameObject.CompareTag("Emu"))
        {
            collision.gameObject.GetComponent<Player>().health -= 20;
            gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Horde"))
        {
            collision.gameObject.GetComponent<Horde>().isDead = true;
            gameObject.SetActive(false);
        }

        // If the bullet collides with an wall...
        if (collision.gameObject.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }
    }

    


}
