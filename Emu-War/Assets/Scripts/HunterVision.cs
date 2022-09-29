using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterVision : MonoBehaviour
{
    #region fields
    [SerializeField]
    //private AIHunterTracking _hunterTracking;
    private GameObject _hunterAi;
    #endregion
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Emu")
        {
            _hunterAi.GetComponent<AIHunterTracking>().InSight = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Emu")
        {
            _hunterAi.GetComponent<AIHunterTracking>().InSight = false;
        }
    }


}
