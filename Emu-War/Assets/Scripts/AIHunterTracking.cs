using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHunterTracking : MonoBehaviour
{
    #region Fields
    public bool inSight;
    public float hunterSpeed;
    [SerializeField]
    private float _rotationSpeed;
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
    }

    // Update is called once per frame
    void Update()
    {
        OnSight();


    }

    public void OnSight()
    {
        if (inSight)
        {
            ShootAtTarget();

        }
        else
        {
            Patrol();

        }
    }

    public void CheckDistance()
    {
        if(Vector2.Distance(transform.position,_currentNode) <= 0.2f)
        {
            _currentNode = _hotpoints.Dequeue();
            Debug.Log(_currentNode);
            _hotpoints.Enqueue(_currentNode);
            _rotateToPoint = true;
            //_rotatePoint = (_currentNode - transform.position).normalized;
        }
    }

    public void Patrol()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

        if (_rotateToPoint)
        {
            //transform.right += (_currentNode -transform.position).normalized;
            //Quaternion rotateTo = Quaternion.LookRotation(Vector3.forward, (_currentNode - transform.position).normalized);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTo, _rotationSpeed * Time.deltaTime);
            _rotateTimer++;

            //Debug.Log("Right: " + transform.right + " Rotate Point: " + _rotatePoint + " Distance: " + Vector3.Distance(transform.right, _rotatePoint));
            Vector3 targetOfRotation = _currentNode - transform.position;
            float angle = Mathf.Atan2(targetOfRotation.y, targetOfRotation.x) * Mathf.Rad2Deg;
            Quaternion rotationQuaternion = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationQuaternion, _rotationSpeed * Time.deltaTime);

            float dot = Vector2.Dot( (targetOfRotation).normalized, this.transform.forward);
            Debug.Log(dot);
            if (_rotateTimer >= 360)
            {
                _rotateToPoint = false;
                _rotateTimer = 0;
                //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.x, transform.eulerAngles.z + 90);
            }
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, _currentNode, hunterSpeed * Time.deltaTime);

            
            //float angle = Mathf.Atan2(_)
            CheckDistance();
        }

    }

    public void ShootAtTarget()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }
}
