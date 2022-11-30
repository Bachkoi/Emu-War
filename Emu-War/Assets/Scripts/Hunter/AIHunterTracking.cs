using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIHunterTracking : MonoBehaviour
{
    #region Fields
    public bool inSight;
    public bool inPeripheral;
    public float hunterSpeed;
    public float temporarayHunterRotation;
    public Animator anim;
    public Quaternion tempRot;
    [SerializeField]
    private float _rotationSpeed;
    private GameObject _playerGameObject;
    private Vector3 _playerPositionAtTimeCaught;
    private bool _playerCaughtInSight;
    private float _hunterRotation;
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
        anim = GetComponent<Animator>();
        temporarayHunterRotation = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        OnSight();
        _hunterRotation = tempRot.z;
        //_hunterRotation = this.gameObject.transform.localRotation.eulerAngles.z;
        hunterRotation(_hunterRotation);
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
                //hunterRotation(RotateHunter(_playerPositionAtTimeCaught));
                //Debug.Log("HAHAHA");
            }
            gameObject.GetComponent<AIHunterShooting>().canFire = true;
            //anim.SetBool("isWalking", false);
            //anim.SetBool("isShooting", false);


        }
        else if(inPeripheral)
        {
            _playerCaughtInSight = true;
            if (gameObject.GetComponent<AIHunterShooting>().FireCycle == false)
            {
                RotateHunter(_playerPositionAtTimeCaught);
                //hunterRotation(RotateHunter(_playerPositionAtTimeCaught));
            }
            gameObject.GetComponent<AIHunterShooting>().canFire = true;
            //anim.SetBool("isWalking", false);
            //anim.SetBool("isShooting", false);


        }
        else if(gameObject.GetComponent<AIHunterShooting>().FireCycle == false)
        {
            Patrol();
            gameObject.GetComponent<AIHunterShooting>().canFire = false;
            anim.SetBool("isWalking", true);
            anim.SetBool("isShooting", false);
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
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationQuaternion, _rotationSpeed * Time.deltaTime);
        //tempRot = Quaternion.Slerp(tempRot, rotationQuaternion, _rotationSpeed * Time.deltaTime);
        //Quaternion tempRot = Quaternion.Slerp(transform.rotation, rotationQuaternion, _rotationSpeed * Time.deltaTime);
        //hunterRotation(tempRot.z);
        float dot = Vector2.Dot((targetOfRotation).normalized, this.transform.forward);
        //hunterRotation(dot);
        return dot;
    }

    public void hunterRotation(float degree)
    {
        float tempDeg = degree;
        temporarayHunterRotation = degree;

        //if (degree < 0.0f && degree > -360.0f)
        //{
        //    tempDeg = 360.0f - MathF.Abs(tempDeg);
        //    temporarayHunterRotation = tempDeg;
        //}
        //else if(degree < -360f)
        //{
        //    tempDeg = tempDeg % 360.0f;
        //    tempDeg = 360.0f - MathF.Abs(tempDeg);
        //    temporarayHunterRotation = tempDeg;
        //}
        //else
        //{
        //    tempDeg = MathF.Abs(tempDeg);
        //    if (tempDeg > 360.0f)
        //    {
        //        tempDeg = tempDeg % 360.0f;
        //        temporarayHunterRotation = tempDeg;
        //    }
        //}
        // Change the Degrees
        if (tempDeg < 20.0f)
        {
            //anim.SetFloat("Direction", (float)0);
            anim.SetInteger("Direction", 0);
        }
        else if (tempDeg > 20.0f && tempDeg < 60.0f)
        {
            //anim.SetFloat("Direction", (float)1);
            anim.SetInteger("Direction", 1);

        }
        else if (tempDeg > 60.0f && tempDeg < 90.0f)
        {
            //anim.SetFloat("Direction", (float)2);
            anim.SetInteger("Direction", 2);

        }
        else if (tempDeg > 90.0f && tempDeg < 110.0f)
        {
            //anim.SetFloat("Direction", (float)3);
            anim.SetInteger("Direction", 3);

        }
        else if (tempDeg > 110.0f && tempDeg < 160.0f)
        {
            //anim.SetFloat("Direction", (float)4);
            anim.SetInteger("Direction", 4);

        }
        else if (tempDeg > 160.0f && tempDeg < 200.0f)
        {
            //anim.SetFloat("Direction", (float)5);
            anim.SetInteger("Direction", 5);

        }
        //else if (tempDeg > 180 && tempDeg < 200)
        //{
        //    anim.SetFloat("Direction", 5);
        //
        //}
        else if (tempDeg > 200.0f && tempDeg < 250.0f)
        {
            //anim.SetFloat("Direction", (float)6);
            anim.SetInteger("Direction", 6);

        }
        else if (tempDeg > 250.0f && tempDeg < 270.0f)
        {
            //anim.SetFloat("Direction", (float)7);
            anim.SetInteger("Direction", 7);

        }
        else if (tempDeg > 270.0f && tempDeg < 300.0f)
        {
            //anim.SetFloat("Direction", (float)8);
            anim.SetInteger("Direction", 8);

        }
        else if (tempDeg > 300.0f && tempDeg < 340.0f)
        {
            //anim.SetFloat("Direction", (float)9);
            anim.SetInteger("Direction", 9);

        }
        else if(tempDeg > 340.0f && tempDeg < 360.0f)
        {
            //anim.SetFloat("Direction", (float)0);
            anim.SetInteger("Direction", 0);

        }
    }

}
