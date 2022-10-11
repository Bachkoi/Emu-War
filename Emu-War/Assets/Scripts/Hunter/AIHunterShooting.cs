using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHunterShooting : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private float _rateOfFire;
    public bool canFire;
    //public GameObject emuToFire;
    private ObjectPooler _objPool;
    private float _counter;
    #endregion Fields

    private void Start()
    {
        _objPool = ObjectPooler.Instance;
        canFire = false;
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
        if (_counter > _rateOfFire)
        {
            GameObject bullet = _objPool.SpawnFromPool("Bullet", transform.position, transform.rotation);
            float randomOffset = (float)Random.Range(-10, 10);
            bullet.transform.rotation *= Quaternion.AngleAxis(randomOffset, Vector3.forward);
            bullet.SetActive(true);
            _counter = 0;
        }
        else
        {
            _counter += Time.deltaTime;
        }

    }
}
