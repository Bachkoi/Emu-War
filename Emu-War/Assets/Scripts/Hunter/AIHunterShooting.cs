using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHunterShooting : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private float _rateOfFire;
    public bool canFire;

    private ObjectPooler _objPool;
    private float _counter;
    
    #region Burst Fire
    private float _burstCounter;
    private float _burstCooldown;
    #endregion Burst Fire

    #endregion Fields

    private void Start()
    {
        //Calling object pooler for bullets
        _objPool = ObjectPooler.Instance;
        canFire = false;
        _burstCounter = 5;
        _burstCooldown = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if(canFire)
        {
            FireGun();
        }
    }

    private void FireGun()
    {
        //If the rate of fire is done
        if (_counter > _rateOfFire)
        {
            //If there are still bursts shots left
            if(_burstCounter > 0)
            {
                //Cooldown for burst
                if(_burstCooldown <= 0)
                {
                    GameObject bullet = _objPool.SpawnFromPool("Bullets", transform.position, transform.rotation);
                    float randomOffset = (float)Random.Range(-10, 10);
                    bullet.transform.rotation *= Quaternion.AngleAxis(randomOffset, Vector3.forward);
                    bullet.SetActive(true);
                    _burstCooldown = 0.3f;
                    _burstCounter--;
                }
                else
                {
                    _burstCooldown-= Time.deltaTime;
                }
                
            }
            else
            {
                _burstCounter = 5;
                _counter = 0;
            }

        }
        else
        {
            _counter += Time.deltaTime;
        }

    }
}
