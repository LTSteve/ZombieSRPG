using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HealthbarEffect : MonoBehaviour
{
    [SerializeField]
    [Range(0f,1f)]
    private float innerWidth = 0.8f;
    [SerializeField]
    [Range(0f,1f)]
    private float shape = 0.75f;
    [SerializeField]
    [Range(0f, 1f)]
    private float healthPercent = 1f;

    private float currentShape { get { return shape * healthPercent; } }

    private MeshFilter meshFilter;

    private Vector3[] vertices;
    private Vector2[] uv;
    private int[] triangles;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();

        var mesh = new Mesh();
        meshFilter.mesh = mesh;
    }

    void Update()
    {
        if (meshFilter == null) Awake();

        _calculateMesh();
        _assignMesh();
    }

    public void UpdateHealth(float percent)
    {
        healthPercent = percent;
    }

    private void _calculateMesh()
    {
        var numVertecies = _roundUp((currentShape + 0.125f) * 4f) * 4;
        numVertecies = numVertecies == 0 ? 4 : numVertecies;

        vertices = new Vector3[numVertecies];
        uv = new Vector2[numVertecies];
        triangles = new int[(numVertecies - 2) * 3];

        vertices[0] = Vector3.zero;

        var vectorAssignmentPosition = -0.125f;

        var currentVertex = 0;
        var currentTriangle = 0;

        do
        {
            //INCREMENT
            vectorAssignmentPosition = Mathf.Clamp01(vectorAssignmentPosition + 0.25f);

            //GET DIRECTIONS
            var directionVector = _positionToDirection(vectorAssignmentPosition);
            var oppositeDirectionVector = new Vector3(-directionVector.x, directionVector.y, directionVector.z);

            //VERTS

            vertices[currentVertex] = directionVector * innerWidth;
            vertices[currentVertex + 1] = directionVector;
            vertices[currentVertex + 2] = oppositeDirectionVector * innerWidth;
            vertices[currentVertex + 3] = oppositeDirectionVector;

            currentVertex += 4;

            //TRIANGLES

            if(currentTriangle == 0)
            {
                //first box

                triangles[currentTriangle] = 0;
                triangles[currentTriangle + 1] = 3;
                triangles[currentTriangle + 2] = 1;

                currentTriangle += 3;

                triangles[currentTriangle] = 0;
                triangles[currentTriangle + 1] = 2;
                triangles[currentTriangle + 2] = 3;

                currentTriangle += 3;
            }
            else
            {
                //right side

                triangles[currentTriangle] = currentVertex - 4;
                triangles[currentTriangle + 1] = currentVertex - 7;
                triangles[currentTriangle + 2] = currentVertex - 3;

                currentTriangle += 3;

                triangles[currentTriangle] = currentVertex - 4;
                triangles[currentTriangle + 1] = currentVertex - 8;
                triangles[currentTriangle + 2] = currentVertex - 7;

                currentTriangle += 3;

                //left side

                triangles[currentTriangle] = currentVertex - 6;
                triangles[currentTriangle + 1] = currentVertex - 1;
                triangles[currentTriangle + 2] = currentVertex - 5;

                currentTriangle += 3;

                triangles[currentTriangle] = currentVertex - 6;
                triangles[currentTriangle + 1] = currentVertex - 2;
                triangles[currentTriangle + 2] = currentVertex - 1;

                currentTriangle += 3;
            }
        }
        while (vectorAssignmentPosition < currentShape);
    }

    private void _assignMesh()
    {
        if (Application.isEditor)
        {
            meshFilter.sharedMesh.vertices = vertices;
            meshFilter.sharedMesh.uv = uv;
            meshFilter.sharedMesh.triangles = triangles;
            meshFilter.sharedMesh.bounds = new Bounds(transform.position, Vector3.one * 1000f);
        }
        else
        {
            meshFilter.mesh.vertices = vertices;
            meshFilter.mesh.uv = uv;
            meshFilter.mesh.triangles = triangles;
            meshFilter.mesh.bounds = new Bounds(transform.position, Vector3.one * 1000f);
        }
    }

    private Vector3 _positionToDirection(float position)
    {
        if(position < currentShape)
        {
            return Quaternion.AngleAxis(position * 180f, Vector3.up) * Vector3.forward;
        }
        else
        {
            var prevPosition = position - 0.25f;//kinda knowing too much here, but whatever

            var prevDirection = Quaternion.AngleAxis(prevPosition * 180f, Vector3.up) * Vector3.forward;
            var nextDirection = Quaternion.AngleAxis(position * 180f, Vector3.up) * Vector3.forward;

            var prevPercent = (position - currentShape) / 0.25f; //again, knowing too much

            return prevDirection * prevPercent + nextDirection * (1f - prevPercent);
        }
    }

    private int _roundUp(float number)
    {
        var intNum = Mathf.RoundToInt(number);

        return intNum == number ? intNum : (intNum + 1);
    }
}