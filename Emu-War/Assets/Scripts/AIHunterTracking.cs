using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHunterTracking : MonoBehaviour
{
    #region Fields
    public bool _inSight;
    #endregion
    #region Properties
    public bool InSight
    {
        get { return _inSight; }
        set { _inSight = value; }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _inSight = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(_inSight)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {

           gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        }
    }
}
