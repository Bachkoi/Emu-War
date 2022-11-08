using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterVision : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private GameObject _hunterAi;
    private int _emuCountInRange = 0;
    private GameObject _playerGameObject;
    private bool _trackingPlayerPosition;
    [SerializeField]
    private List<GameObject> _emusInSight;
    #endregion

    private void Start()
    {
        _playerGameObject = GameObject.FindGameObjectsWithTag("Emu")[0];
        _trackingPlayerPosition = true;
        _emusInSight = new List<GameObject>();
    }
    public void Update()
    {
        if(_emuCountInRange > 0)
        {
            GameObject closestEmu = GetClosestEmu();
            Debug.Log(closestEmu.name);
            _hunterAi.GetComponent<AIHunterTracking>().InSight = true;
            _hunterAi.GetComponent<AIHunterTracking>().PlayerPositionAtTimeCaught = closestEmu.transform.position;
        }
        else
        {
            _hunterAi.GetComponent<AIHunterTracking>().InSight = false;
        }
    }

    private GameObject GetClosestEmu()
    {
        GameObject closestEmu = _emusInSight[0];
        Vector3 hunterPosition = gameObject.transform.position;
        for(int i = 1; i <_emusInSight.Count;i++)
        {
            //Get the Emu the closest to the Hunter within vision
            if(Vector2.Distance(hunterPosition,closestEmu.transform.position) > Vector2.Distance(hunterPosition,_emusInSight[i].transform.position))
            {
                closestEmu = _emusInSight[i];
            }
        }
        return closestEmu;
    }

    private void CheckEmuStatus()
    {
        foreach (var emu in _emusInSight)
        {
            if(!emu.activeSelf)
            {
                _emusInSight.Remove(emu);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Emu" || collision.gameObject.tag == "Horde")
        {
            //Debug.Log(collision.tag);
            _emuCountInRange++;
            _emusInSight.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Emu" || collision.gameObject.tag == "Horde")
        {
            _emuCountInRange--;
            if(_emusInSight.Contains(collision.gameObject))
            {
                _emusInSight.Remove(collision.gameObject);
            }
        }
    }


}
