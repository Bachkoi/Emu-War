using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterPeripheralVision : MonoBehaviour
{
    #region fields
    [SerializeField]
    private GameObject _hunterAi;
    private int _emusInRange = 0;
    private GameObject _playerGameObject;
    private bool _trackingPlayerPosition;
    #endregion

    private void Start()
    {
        _playerGameObject = GameObject.FindGameObjectsWithTag("Emu")[0];
    }
    public void Update()
    {
        if (_emusInRange > 0)
        {

            _hunterAi.GetComponent<AIHunterTracking>().InPeripheral = true;
            _hunterAi.GetComponent<AIHunterTracking>().PlayerPositionAtTimeCaught = _playerGameObject.transform.position;


        }
        else
        {
            _hunterAi.GetComponent<AIHunterTracking>().InPeripheral = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Emu")
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
