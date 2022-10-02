using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHunterTracking : MonoBehaviour
{
    #region Fields
    public bool inSight;
    [SerializeField]
    private List<Vector3> _spots;
    private Queue<Vector3> _hotpoints;
    private float _travelDistance = 4.0f;
    [SerializeField]
    private float _randomSpotOdds;
    private Vector3 _currentNode;
    public float hunterSpeed;
    #endregion
    #region Properties
    public bool InSight
    {
        get { return inSight; }
        set { inSight = value; }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        inSight = false;
        _hotpoints = new Queue<Vector3>(_spots);
        _currentNode = _hotpoints.Dequeue();
        _hotpoints.Enqueue(_currentNode);
    }

    // Update is called once per frame
    void Update()
    {
        OnSight();
        transform.position = Vector2.MoveTowards(transform.position, _currentNode, hunterSpeed * Time.deltaTime);
        //transform.LookAt(_currentNode,Vector3.left);
        transform.right = _currentNode - transform.position;
        CheckDistance();
    }

    public void OnSight()
    {
        if (inSight)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else
        {

            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public void CheckDistance()
    {
        if(Vector2.Distance(transform.position,_currentNode) <= 0.2f)
        {
            _currentNode = _hotpoints.Dequeue();
            Debug.Log(_currentNode);
            _hotpoints.Enqueue(_currentNode);
        }
    }
}
