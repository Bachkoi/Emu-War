using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHunterTracking : MonoBehaviour
{
    #region Fields
    public bool inSight;
    public bool inPeripheral;
    public float hunterSpeed;
    [SerializeField]
    private float _rotationSpeed;
    private GameObject _playerGameObject;
    private Vector3 _playerPositionAtTimeCaught;
    private bool _playerCaughtInSight;
    private float _hunterZRotation;
    private Quaternion _currentRotation;
    #region Patrol Points
    [SerializeField]
    private List<Vector3> _spots;
    private Queue<Vector3> _hotpoints;
    [SerializeField]
    private float _randomSpotOdds;
    private Vector3 _currentNode;
    private bool _rotateToPoint;
    private Vector3 _rotatePoint;
    private int _rotateTimer;
    #endregion Patrol Points

    #endregion
    #region Properties
    public bool InSight
    {
        get { return inSight; }
        set { inSight = value; }
    }
    public bool InPeripheral
    {
        get { return inPeripheral; }
        set { inPeripheral = value; }
    }
    public Vector3 PlayerPositionAtTimeCaught
    {
        set { _playerPositionAtTimeCaught = value; }
    }
    public float HunterZRotation
    {
        get { return _hunterZRotation; }
        set { _hunterZRotation = value; }
    }
    public Quaternion CurrentRotation
    {
        get { return _currentRotation; }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        inSight = false;
        _hotpoints = new Queue<Vector3>(_spots);
        _currentNode = _hotpoints.Dequeue();
        _hotpoints.Enqueue(_currentNode);
        _rotateToPoint = true;
        _rotateTimer = 0;
        _playerGameObject = GameObject.FindGameObjectsWithTag("Emu")[0];
        _playerPositionAtTimeCaught = Vector3.zero;
        _playerCaughtInSight = false;
        _hunterZRotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        OnSight();
    }

    public void OnSight()
    {

        //If Emu is in sight, shoot, otherwise patrol
        if (inSight)
        {
            _playerCaughtInSight = true;

            if (gameObject.GetComponent<AIHunterShooting>().FireCycle == false)
            {
                RotateHunter(_playerPositionAtTimeCaught);
                //Debug.Log("HAHAHA");
            }

            gameObject.GetComponent<AIHunterShooting>().canFire = true;
        }
        else if(inPeripheral)
        {
            _playerCaughtInSight = true;
            if (gameObject.GetComponent<AIHunterShooting>().FireCycle == false)
            {
                RotateHunter(_playerPositionAtTimeCaught);
            }
            gameObject.GetComponent<AIHunterShooting>().canFire = true;
        }

        else if(gameObject.GetComponent<AIHunterShooting>().FireCycle == false)
        {
            //Set origin and rotation to Raycast
            //pass position and angle to raycast vision
            Patrol();
            gameObject.GetComponent<AIHunterShooting>().canFire = false;
        }
    }

    public void Patrol()
    {
        if (_rotateToPoint || _playerCaughtInSight)
        {
            _rotateTimer++;

            //logic for later STILL IN PROGRESS
            float DotProduct = RotateHunter(_currentNode);
            if (_rotateTimer >= 360)
            {
                _rotateToPoint = false;
                _playerCaughtInSight = false;
                _rotateTimer = 0;
                //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.x, transform.eulerAngles.z + 90);
            }
        }

        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _currentNode, hunterSpeed * Time.deltaTime);
            CheckDistance();
        }

    }

    //Checks current distance to waypoint. If close enough, move to rotating.
    public void CheckDistance()
    {
        //If close enough to the current waypoint, pick a new wavepoint and rotate towards it
        if(Vector2.Distance(transform.position,_currentNode) <= 0.2f)
        {
            _currentNode = _hotpoints.Dequeue();
            //Debug.Log(_currentNode);
            _hotpoints.Enqueue(_currentNode);
            _rotateToPoint = true;
            //_rotatePoint = (_currentNode - transform.position).normalized;
        }
    }

    //Rotate player towards waypoint
    public float RotateHunter(Vector3 focalPoint)
    {
        Vector3 targetOfRotation = focalPoint - transform.position;
        float angle = Mathf.Atan2(targetOfRotation.y, targetOfRotation.x) * Mathf.Rad2Deg;
        Quaternion rotationQuaternion = Quaternion.AngleAxis(angle, Vector3.forward);
        _currentRotation = Quaternion.Slerp(_currentRotation, rotationQuaternion, _rotationSpeed * Time.deltaTime);
        _hunterZRotation = _currentRotation.eulerAngles.z;
        Debug.Log(_hunterZRotation);
        float dot = Vector2.Dot((targetOfRotation).normalized, this.transform.forward);
        return dot;
    }

}
