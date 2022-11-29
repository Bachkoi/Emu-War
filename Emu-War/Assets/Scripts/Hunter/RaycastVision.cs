using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastVision : MonoBehaviour
{
    #region Fields
    [SerializeField]
    //What layers should the raycast detect
    private LayerMask _layerMask;
    private Vector3 _origin;
    private Mesh _visionMesh;
    private float _startingAngle;

    #region Raycast Variables
    //Total angle measurement of the raycasts
    private float _fov = 45f;
    //how many rays per hunter
    private int _rayCount = 20;
    //how far each ray should travel
    private float _viewDistance = 10f;
    #endregion Raycast Variables

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
        _startingAngle = 0;
        gameObject.GetComponent<MeshFilter>().mesh = _visionMesh;
        _foundTarget = false;
    }

    // Update is called once per frame
    private void Update()
    {
        _startingAngle = _hunterAi.GetComponent<Transform>().rotation.eulerAngles.z + _fov / 2f;
        _origin = _hunterAi.GetComponent<Transform>().position;
    }
    //Late update is to make sure the proper angles are updated before creating a mesh
    void LateUpdate()
    {
        CreateMesh();
    }

    private void CreateMesh()
    { 

        float angle = _startingAngle;
        float angleIncrement = _fov / _rayCount;


        Vector3[] vertices = new Vector3[_rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[_rayCount * 3];


        int vertexIndex = 1;
        int triangleIndex = 0;
        vertices[0] = _origin;

        _foundTarget = false;

        for (int i = 0; i <= _rayCount; i++)
        {
            Vector3 vertex;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(_origin, GetVectorFromAngle(angle), _viewDistance,_layerMask);

            if(raycastHit2D.collider == null )
            {
                //raycast hit nothing, set vertex to furthest distance
                vertex = _origin + (GetVectorFromAngle(angle) * _viewDistance);
            }
            //raycast hit something, set the distance of this vertext to the hit
            else
            {
               //logic for if an emu is in sight
                if(raycastHit2D.collider.tag == "Emu" || raycastHit2D.collider.tag == "Horde")
                {
                    _hunterAi.GetComponent<AIHunterTracking>().InSight = true;
                    _hunterAi.GetComponent<AIHunterTracking>().PlayerPositionAtTimeCaught = raycastHit2D.collider.gameObject.transform.position;
                    _foundTarget = true;
                }
                vertex = raycastHit2D.point;
            }

            vertices[vertexIndex] = vertex;

            //creating the triangles that will form the vision mesh
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

        //if an emu or hordeling has not been detected there is no need to trigger the shooting command
        if (!_foundTarget)
        {
            _hunterAi.GetComponent<AIHunterTracking>().InSight = false;
        }

        _visionMesh.vertices = vertices;
        _visionMesh.uv = uv;
        _visionMesh.triangles = triangles;
    }

    //Return a Vector 3 based on a given angle
    public Vector3 GetVectorFromAngle(float angle)
    {
        float angleRad = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }


}
