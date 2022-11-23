using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastVision : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private LayerMask _layerMask;
    private Vector3 _origin;
    private Mesh _visionMesh;
    private float _startingAngle;
    private float _fov;
    [SerializeField]
    private GameObject _hunterAi;
    private bool _foundTarget;
    #endregion Fields

    #region Properties
    #endregion Properties
    // Start is called before the first frame update
    void Start()
    {
        _visionMesh = new Mesh();
        _origin = Vector3.zero;
        _fov = 60f;
        _startingAngle = 0;
        gameObject.GetComponent<MeshFilter>().mesh = _visionMesh;
        _foundTarget = false;
        //CreateMesh();
    }

    // Update is called once per frame
    private void Update()
    {
        _startingAngle = _hunterAi.GetComponent<Transform>().rotation.eulerAngles.z + _fov / 2f;// + 90;
        //_startingAngle = _fov - this.gameObject.GetComponentInParent<Transform>().rotation.eulerAngles.z / 2f;
        _origin = _hunterAi.GetComponent<Transform>().position;
    }
    void LateUpdate()
    {

        Debug.Log(_startingAngle + "    " + this.gameObject.GetComponentInParent<Transform>().rotation.eulerAngles.z);
        CreateMesh();
        
    }

    private void CreateMesh()
    {

        int rayCount = 20;
        float angle = _startingAngle;
        float angleIncrement = _fov / rayCount;
        float viewDistance = 10f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];


        int vertexIndex = 1;
        int triangleIndex = 0;
        vertices[0] = _origin;

        _foundTarget = false;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(_origin, GetVectorFromAngle(angle), viewDistance,_layerMask);

            if(raycastHit2D.collider == null )
            {
                vertex = _origin + (GetVectorFromAngle(angle) * viewDistance);
            }
            else
            {
                if(raycastHit2D.collider.tag == "Emu")
                {
                    Debug.Log("Found him");
                    _hunterAi.GetComponent<AIHunterTracking>().InSight = true;
                    _hunterAi.GetComponent<AIHunterTracking>().PlayerPositionAtTimeCaught = raycastHit2D.collider.gameObject.transform.position;
                    _foundTarget = true;
                }
                vertex = raycastHit2D.point;
            }

            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrement;
        }

        if (!_foundTarget)
        {
            _hunterAi.GetComponent<AIHunterTracking>().InSight = true;
        }

        _visionMesh.vertices = vertices;
        _visionMesh.uv = uv;
        _visionMesh.triangles = triangles;
    }


    public Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public float GetAngleFromVector(Vector3 vector)
    {
        vector = vector.normalized;

        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        if(angle < 0)
        {
            angle += 360;
        }

        return angle;
    }

    public void SetOrigin(Vector3 origin)
    {
        _origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        _startingAngle = GetAngleFromVector(aimDirection) + (_fov/2f);
    }


}
