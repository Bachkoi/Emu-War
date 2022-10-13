using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private float _bulletSpeed;
    private int _lifetimeCount;
    #endregion Fields

    private void Start()
    {
        _lifetimeCount = 0;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * _bulletSpeed * Time.deltaTime;
        _lifetimeCount++;
        if(_lifetimeCount > 300)
        {
            _lifetimeCount = 0;
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);

        // If the Wheat collides with an emu...
        if (collision.gameObject.CompareTag("Emu"))
        {
            collision.gameObject.GetComponent<Player>().health -= 20;
        }
    }
}
