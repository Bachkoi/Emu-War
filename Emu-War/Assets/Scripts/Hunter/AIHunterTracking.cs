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


    private Animator _animator;

    #endregion Fields

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
        //_animator = gameObject.GetComponent<Animator>();
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
            //gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            //_animator.SetBool("isWalking", false);
            gameObject.GetComponent<AIHunterShooting>().canFire = true;
        }
        else
        {
            Patrol();
            gameObject.GetComponent<AIHunterShooting>().canFire = false;
        }
    }

    public void Patrol()
    {
        //gameObject.GetComponent<SpriteRenderer>().color = Color.blue;

        if (_rotateToPoint)
        {

            _rotateTimer++;

            //logic for later STILL IN PROGRESS
            float DotProduct = RotateHunter();
            if (_rotateTimer >= 360)
            {
                _rotateToPoint = false;
                _rotateTimer = 0;
                //_animator.SetBool("isWalking", true);
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
            Debug.Log(_currentNode);
            _hotpoints.Enqueue(_currentNode);
            _rotateToPoint = true;
            //_animator.SetBool("isWalking", false);
        }
    }

    //Rotate player towards waypoint
    public float RotateHunter()
    {
        Vector3 targetOfRotation = _currentNode - transform.position;
        float angle = Mathf.Atan2(targetOfRotation.y, targetOfRotation.x) * Mathf.Rad2Deg;
        Quaternion rotationQuaternion = Quaternion.AngleAxis(angle, Vector3.forward);
        gameObject.GetComponentInChildren<Transform>().rotation = Quaternion.Slerp(transform.rotation, rotationQuaternion, _rotationSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotationQuaternion, _rotationSpeed * Time.deltaTime);
        float dot = Vector2.Dot((targetOfRotation).normalized, gameObject.GetComponentInChildren<Transform>().forward);
        //float dot = Vector2.Dot((targetOfRotation).normalized, this.transform.forward);
        return dot;
    }

}
