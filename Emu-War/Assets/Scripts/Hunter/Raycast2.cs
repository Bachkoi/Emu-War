using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast2 : MonoBehaviour
{
    #region Fields
    public float viewRadius;
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public float meshResolution;
    #endregion Fields

    public void Update()
    {
        DrawFieldOfView();
    }
    void DrawFieldOfView()
    {
        int rayCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / rayCount;

        for(int i = 0; i < rayCount; i++)
        {
            float angle = transform.eulerAngles.z - viewAngle/2 + stepAngleSize*i;
            //Debug.Log("Looking at " + (transform.position + DirFromAngle(angle)));
           // Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle) * viewRadius, Color.green);
        }
    }
    
    public Vector3 DirFromAngle (float angleInDegrees)
    {
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(angleInRadians), Mathf.Cos(angleInRadians), 0);
    }
}
