using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterVision : MonoBehaviour
{
    #region fields
    [SerializeField]
    private GameObject _hunterAi;
    private int _emusInRange = 0;
    #endregion

    public void Update()
    {
        if(_emusInRange > 0)
        {
            _hunterAi.GetComponent<AIHunterTracking>().InSight = true;
            //Debug.Log("Hey in here!");
        }
        else
        {
            _hunterAi.GetComponent<AIHunterTracking>().InSight = false;
            //Debug.Log("Not Here :(");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Emu")
        {
            _emusInRange++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Emu")
        {
           _emusInRange--;
        }
    }


}
